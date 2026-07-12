# GuardianOS Deployment Guide

## Prerequisites

- .NET 9 Runtime
- PostgreSQL 14+
- Windows Server 2019+ (for service deployment)
- Administrator privileges
- SSL/TLS certificates

## Production Configuration

### 1. Update appsettings.Production.json

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=prod-db.example.com;Port=5432;Database=guardianOS;Username=guardian;Password=SECURE_PASSWORD"
  },
  "JwtSettings": {
    "SecretKey": "VERY_LONG_SECURE_KEY_AT_LEAST_32_CHARACTERS",
    "Issuer": "GuardianOS",
    "Audience": "GuardianOSClients",
    "ExpirationMinutes": 60,
    "RefreshTokenExpirationDays": 7
  },
  "Encryption": {
    "MasterKey": "SECURE_ENCRYPTION_KEY_32_CHARACTERS"
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  }
}
```

### 2. Database Setup

```bash
# Connect to PostgreSQL
psql -U postgres -h prod-db.example.com

# Create database
CREATE DATABASE guardianOS;
CREATE USER guardian WITH PASSWORD 'SECURE_PASSWORD';
GRANT ALL PRIVILEGES ON DATABASE guardianOS TO guardian;

# Apply migrations
cd Guardian.API
dotnet ef database update --configuration Release
```

## API Deployment

### Azure App Service

1. **Create Resource Group**
```bash
az group create --name GuardianOS --location eastus
```

2. **Create App Service Plan**
```bash
az appservice plan create \
  --name GuardianOSPlan \
  --resource-group GuardianOS \
  --sku B2 \
  --is-linux
```

3. **Create Web App**
```bash
az webapp create \
  --resource-group GuardianOS \
  --plan GuardianOSPlan \
  --name guardian-os-api \
  --runtime "DOTNET|9.0"
```

4. **Deploy Application**
```bash
cd Guardian.API
dotnet publish -c Release
az webapp up --resource-group GuardianOS --name guardian-os-api
```

### AWS EC2

1. **Launch Instance**
   - AMI: Windows Server 2022
   - Instance Type: t3.medium
   - Security Group: Allow ports 443, 80, 5432

2. **Install Prerequisites**
```powershell
# Install .NET 9 Runtime
Invoke-WebRequest -Uri "https://dot.net/v1/dotnet-install.ps1" -OutFile dotnet-install.ps1
.\dotnet-install.ps1 -Runtime dotnet

# Install IIS
Add-WindowsFeature Web-Server
```

3. **Deploy Application**
```powershell
# Copy application files
Copy-Item "C:\path\to\publish\*" -Destination "C:\inetpub\wwwroot\guardian-api" -Recurse

# Create IIS Application
New-WebApplication -Site "Default Web Site" -Name "guardian-api" -PhysicalPath "C:\inetpub\wwwroot\guardian-api"
```

## Windows Service Deployment

### Installation

1. **Build Release**
```bash
cd Guardian.Service
dotnet publish -c Release -o "C:\GuardianOS\Service"
```

2. **Install Service**
```powershell
# Run as Administrator
sc create GuardianOS `
  binPath="C:\GuardianOS\Service\Guardian.Service.exe" `
  start=auto `
  displayName="GuardianOS Security Service" `
  obj="NT AUTHORITY\SYSTEM"
```

3. **Start Service**
```powershell
net start GuardianOS
```

### Verify Installation

```powershell
# Check service status
Get-Service -Name GuardianOS

# View service logs
Get-EventLog -LogName System -Source GuardianOS | Select-Object -Last 10
```

## SSL/TLS Configuration

### Generate Self-Signed Certificate (Development)

```powershell
$cert = New-SelfSignedCertificate `
  -CertStoreLocation "cert:\LocalMachine\My" `
  -DnsName "api.guardianOS.local" `
  -FriendlyName "GuardianOS API"
```

### Configure HTTPS in Program.cs

```csharp
var builder = WebApplicationBuilder.CreateBuilder(args);

builder.Services.AddHttpsRedirection(options =>
{
    options.HttpsPort = 443;
});

var app = builder.Build();
app.UseHttpsRedirection();
```

## Monitoring & Logging

### Configure Serilog

```csharp
builder.Host.UseSerilog((context, services, loggerConfig) =>
    loggerConfig
        .MinimumLevel.Information()
        .Enrich.FromLogContext()
        .WriteTo.Console()
        .WriteTo.File(
            path: "/var/log/guardianOS/api-.txt",
            rollingInterval: RollingInterval.Day,
            retainedFileCountLimit: 30,
            outputTemplate: "[{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz}] [{Level:u3}] {Message:lj}{NewLine}{Exception}")
        .WriteTo.ApplicationInsights(
            services.GetRequiredService<TelemetryClient>(),
            TelemetryConverter.Events));
```

### Azure Application Insights

```bash
# Create Application Insights resource
az monitor app-insights component create \
  --app guardian-os-insights \
  --location eastus \
  --resource-group GuardianOS \
  --application-type web
```

## Health Checks

### Add Health Check Endpoint

```csharp
builder.Services.AddHealthChecks()
    .AddDbContextCheck<GuardianDbContext>()
    .AddCheck("API Health", () => 
        HealthCheckResult.Healthy("API is running"));

var app = builder.Build();
app.MapHealthChecks("/health");
```

## Backup Strategy

### Database Backups

```bash
# Daily backup
0 2 * * * /usr/bin/pg_dump -h prod-db.example.com -U guardian guardianOS | gzip > /backups/db_$(date +\%Y-\%m-\%d).sql.gz

# Upload to S3
0 3 * * * aws s3 sync /backups s3://guardianOS-backups --delete
```

### Configuration Backups

```bash
# Backup appsettings
tar -czf /backups/config_$(date +%Y-%m-%d).tar.gz /etc/guardianOS/
aws s3 cp /backups/config_$(date +%Y-%m-%d).tar.gz s3://guardianOS-backups/
```

## Security Hardening

### Firewall Rules

```powershell
# Allow HTTPS inbound
New-NetFirewallRule -DisplayName "HTTPS" -Direction Inbound -Action Allow -Protocol TCP -LocalPort 443

# Allow API communication
New-NetFirewallRule -DisplayName "API" -Direction Inbound -Action Allow -Protocol TCP -LocalPort 5000

# Deny other inbound
Set-NetFirewallProfile -DefaultInboundAction Block
```

### Database Security

```sql
-- Create read-only user for backups
CREATE USER backup_user WITH PASSWORD 'backup_password';
GRANT CONNECT ON DATABASE guardianOS TO backup_user;
GRANT USAGE ON SCHEMA public TO backup_user;
GRANT SELECT ON ALL TABLES IN SCHEMA public TO backup_user;

-- Enforce SSL
ALTER DATABASE guardianOS SET ssl = on;
```

## Troubleshooting

### API Won't Start
1. Check logs: `Get-EventLog -LogName Application | Select-Object -Last 20`
2. Verify database connection: `Test-NetConnection -ComputerName db.example.com -Port 5432`
3. Check ports: `netstat -ano | findstr :5000`

### Service Won't Start
1. Check service logs: `Get-EventLog -LogName System -Source GuardianOS`
2. Verify permissions: Service should run as SYSTEM
3. Check application logs: `C:\ProgramData\GuardianOS\Logs\`

### Database Connection Fails
1. Verify PostgreSQL is running
2. Check firewall: `Test-NetConnection -ComputerName prod-db.example.com -Port 5432`
3. Verify credentials in appsettings
4. Test connection: `psql -h prod-db.example.com -U guardian -d guardianOS`
