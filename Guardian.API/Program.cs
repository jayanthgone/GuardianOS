using Guardian.API.Hubs;
using Guardian.Common.Security;
using Guardian.Database;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using System.Text;

var builder = WebApplicationBuilder.CreateBuilder(args);

// Configure Serilog
builder.Host.UseSerilog((context, services, loggerConfig) =>
    loggerConfig
        .MinimumLevel.Information()
        .WriteTo.Console()
        .WriteTo.File(
            path: "logs/guardian-api-.txt",
            rollingInterval: RollingInterval.Day,
            outputTemplate: "[{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz}] [{Level:u3}] {Message:lj}{NewLine}{Exception}"));

// Add services to the container
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add DbContext
builder.Services.AddDbContext<GuardianDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// Add security services
builder.Services.AddSingleton<IPasswordHasher>(new Argon2PasswordHasher());
builder.Services.AddSingleton<IEncryptionService>(
    new AesEncryptionService(builder.Configuration["Encryption:MasterKey"] ?? "guardian-default-key"));

// Add CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", builder =>
    {
        builder.AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader();
    });
});

// Add authentication
var jwtSettings = builder.Configuration.GetSection("JwtSettings");
var secretKey = jwtSettings["SecretKey"];
if (string.IsNullOrEmpty(secretKey))
    throw new InvalidOperationException("JWT SecretKey not configured");

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey)),
            ValidateIssuer = true,
            ValidIssuer = jwtSettings["Issuer"],
            ValidateAudience = true,
            ValidAudience = jwtSettings["Audience"],
            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero
        };
        
        // SignalR support for JWT
        options.Events = new JwtBearerEvents
        {
            OnMessageReceived = context =>
            {
                var accessToken = context.Request.Query["access_token"];
                var path = context.HttpContext.Request.Path;
                
                if (!string.IsNullOrEmpty(accessToken) && path.StartsWithSegments("/hubs"))
                {
                    context.Token = accessToken;
                }
                return Task.CompletedTask;
            }
        };
    });

// Add SignalR
builder.Services.AddSignalR();

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("AllowAll");
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapHub<NotificationHub>("/hubs/notifications");

// Run migrations
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<GuardianDbContext>();
    dbContext.Database.Migrate();
}

app.Run();
