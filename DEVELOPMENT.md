# GuardianOS Development Guide

## Project Setup

### Prerequisites
- .NET 9 SDK
- PostgreSQL 14+
- Visual Studio 2022 or JetBrains Rider
- Node.js 18+ (for web dashboard)
- Flutter SDK (for mobile app)

### Initial Setup

1. **Clone Repository**
```bash
git clone https://github.com/jayanthgone/GuardianOS.git
cd GuardianOS
```

2. **Create PostgreSQL Database**
```bash
psql -U postgres
CREATE DATABASE guardianOS;
CREATE USER guardian WITH PASSWORD 'guardian_password';
ALTER ROLE guardian SET client_encoding TO 'utf8';
ALTER ROLE guardian SET default_transaction_isolation TO 'read committed';
ALTER ROLE guardian SET default_transaction_deferrable TO on;
ALTER ROLE guardian SET default_transaction_deferrable TO off;
GRANT ALL PRIVILEGES ON DATABASE guardianOS TO guardian;
```

3. **Update Connection String**
Edit `Guardian.API/appsettings.json`:
```json
"ConnectionStrings": {
  "DefaultConnection": "Host=localhost;Port=5432;Database=guardianOS;Username=guardian;Password=guardian_password"
}
```

4. **Run Entity Framework Migrations**
```bash
cd Guardian.API
dotnet ef database update
```

5. **Build Solution**
```bash
dotnet build GuardianOS.sln
```

## Architecture Decisions

### 1. Layered Architecture
We use a clean, layered architecture to separate concerns:
- **Guardian.Shared**: DTOs (Data Transfer Objects) for cross-project communication
- **Guardian.Common**: Shared utilities and security services
- **Guardian.Database**: Entity Framework Core models and database context
- **Guardian.API**: RESTful API and SignalR hubs
- **Guardian.Service**: Windows background service
- **Guardian.Desktop**: WPF desktop application

**Rationale**: This separation ensures that each layer has a single responsibility, making the codebase easier to maintain, test, and extend.

### 2. MVVM Pattern for Desktop
We use MVVM (Model-View-ViewModel) with CommunityToolkit.MVVM for the desktop application.

**Rationale**: 
- Clean separation between UI and business logic
- Easy to test ViewModels independently
- Data binding reduces boilerplate code
- CommunityToolkit provides code generation for observable properties

### 3. Async/Await Throughout
All I/O operations use async/await patterns.

**Rationale**:
- Prevents blocking threads
- Improves application responsiveness
- Scales better with many concurrent operations

### 4. JWT Authentication
We use JWT tokens with refresh tokens for stateless authentication.

**Rationale**:
- Scales horizontally (no session state needed)
- Works well with microservices
- Compatible with mobile and web clients
- Industry standard

### 5. SignalR for Real-time Updates
We use SignalR for real-time notifications from server to clients.

**Rationale**:
- Automatic fallback to polling if WebSocket unavailable
- Efficient bidirectional communication
- Easy integration with .NET applications
- Supports groups and broadcast messaging

## Code Quality Standards

### 1. SOLID Principles

#### Single Responsibility Principle
Each class should have one reason to change.
```csharp
// Good: Separated concerns
public class UserService { /* user logic */ }
public class PasswordHasher : IPasswordHasher { /* hashing */ }

// Bad: Multiple responsibilities
public class UserManager { /* everything */ }
```

#### Open/Closed Principle
Classes should be open for extension, closed for modification.
```csharp
// Good: Use interfaces
public interface IEncryptionService { }
public class AesEncryptionService : IEncryptionService { }

// Bad: Hard dependency
public class DataService {
    private AesEncryptionService _encryption = new();
}
```

#### Liskov Substitution Principle
Derived classes should be substitutable for base classes.
```csharp
// Good: Interface contract honored
public interface IRemoteCommand { Task Execute(); }
public class LockComputerCommand : IRemoteCommand { public async Task Execute() { } }
```

#### Interface Segregation Principle
Clients should not depend on interfaces they don't use.
```csharp
// Good: Focused interfaces
public interface IPasswordHasher { string HashPassword(string); }
public interface IEncryptionService { string Encrypt(string); }

// Bad: Fat interface
public interface ICryptography { /* everything */ }
```

#### Dependency Inversion Principle
Depend on abstractions, not concrete implementations.
```csharp
// Good: Depends on interface
public class AuthService {
    private readonly IPasswordHasher _hasher;
    public AuthService(IPasswordHasher hasher) => _hasher = hasher;
}

// Bad: Depends on concrete class
public class AuthService {
    private readonly Argon2PasswordHasher _hasher = new();
}
```

### 2. Dependency Injection
All dependencies should be injected via constructors.

```csharp
public class UserService {
    private readonly GuardianDbContext _dbContext;
    private readonly ILogger<UserService> _logger;

    // Constructor injection
    public UserService(GuardianDbContext dbContext, ILogger<UserService> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }
}
```

### 3. XML Documentation
All public classes, methods, and properties must have XML documentation.

```csharp
/// <summary>
/// Gets or sets the user's email address.
/// </summary>
public string Email { get; set; }

/// <summary>
/// Authenticates a user with email and password.
/// </summary>
/// <param name="email">User's email address.</param>
/// <param name="password">User's password (will be hashed).</param>
/// <returns>Authentication response with JWT tokens.</returns>
public async Task<AuthenticationResponse> AuthenticateAsync(string email, string password)
{
    // implementation
}
```

### 4. Logging Standards
Use structured logging with context information.

```csharp
// Good: Structured logging
_logger.LogInformation("User {UserId} logged in from device {DeviceId}", userId, deviceId);
_logger.LogError(ex, "Failed to process command {CommandId}", commandId);

// Bad: String concatenation
_logger.LogInformation("User " + userId + " logged in");
```

### 5. Configuration Management
Use appsettings.json and dependency injection for configuration.

```csharp
// Good: Configuration from appsettings.json
var jwtSettings = configuration.GetSection("JwtSettings");
var secretKey = jwtSettings["SecretKey"];

// Bad: Hardcoded values
const string SecretKey = "my-secret-key";
```

## Development Workflow

### 1. Creating a New Feature

1. **Create Issue** describing the feature
2. **Create Feature Branch**
```bash
git checkout -b feature/your-feature-name
```
3. **Implement Feature**
   - Follow SOLID principles
   - Add XML documentation
   - Write tests
   - Use async/await for I/O
4. **Test Locally**
```bash
dotnet test
```
5. **Push and Create Pull Request**

### 2. Code Review Checklist

- [ ] Code follows SOLID principles
- [ ] All public methods have XML documentation
- [ ] No hardcoded values (use configuration)
- [ ] Async/await for I/O operations
- [ ] Proper error handling and logging
- [ ] Unit tests for business logic
- [ ] No SQL injection vulnerabilities
- [ ] Appropriate security measures

### 3. Security Checklist

- [ ] No passwords in logs or configuration
- [ ] Input validation on all endpoints
- [ ] Output encoding to prevent XSS
- [ ] CSRF tokens for state-changing operations
- [ ] SQL injection prevention via parameterized queries
- [ ] Authentication and authorization checks
- [ ] HTTPS/TLS enforcement
- [ ] Sensitive data encryption

## Database Migrations

### Creating a Migration
```bash
cd Guardian.API
dotnet ef migrations add MigrationName
```

### Applying Migrations
```bash
dotnet ef database update
```

### Removing Last Migration
```bash
dotnet ef migrations remove
```

## Testing

### Running Tests
```bash
dotnet test
```

### Writing Tests
```csharp
[TestClass]
public class UserServiceTests
{
    [TestMethod]
    public async Task AuthenticateAsync_WithValidCredentials_ReturnsToken()
    {
        // Arrange
        var userService = new UserService(mockDbContext, mockLogger);
        
        // Act
        var result = await userService.AuthenticateAsync("test@example.com", "password");
        
        // Assert
        Assert.IsNotNull(result.AccessToken);
    }
}
```

## Deployment

### API Deployment
```bash
cd Guardian.API
dotnet publish -c Release
# Deploy to Azure App Service, AWS EC2, or on-premises
```

### Service Deployment
```bash
sc create GuardianOS binPath="C:\path\to\Guardian.Service.exe" start=auto
net start GuardianOS
```

### Desktop Application Deployment
```bash
cd Guardian.Desktop
dotnet publish -c Release -p:PublishProfile=FolderProfile
# Create installer or distribute via Windows Store
```

## Troubleshooting

### Database Connection Issues
1. Verify PostgreSQL is running
2. Check connection string in appsettings.json
3. Verify user permissions
4. Check firewall rules

### JWT Token Issues
1. Verify secret key is configured
2. Check token expiration
3. Verify issuer and audience match

### Windows Service Issues
1. Run Event Viewer to check service logs
2. Verify service is running: `net start GuardianOS`
3. Check log files in `C:\ProgramData\GuardianOS\Logs\`

## Useful Commands

```bash
# Build solution
dotnet build

# Run tests
dotnet test

# Run API locally
cd Guardian.API && dotnet run

# Run desktop app
cd Guardian.Desktop && dotnet run

# Format code
dotnet format

# Check code quality
dotnet tool install -g dotnet-codeanalysis
```

## Resources

- [Microsoft .NET Documentation](https://docs.microsoft.com/en-us/dotnet/)
- [ASP.NET Core Documentation](https://docs.microsoft.com/en-us/aspnet/core/)
- [Entity Framework Core](https://docs.microsoft.com/en-us/ef/core/)
- [SignalR Documentation](https://docs.microsoft.com/en-us/aspnet/core/signalr/)
- [WPF Documentation](https://docs.microsoft.com/en-us/dotnet/desktop/wpf/)
- [MVVM Community Toolkit](https://github.com/CommunityToolkit/dotnet)
