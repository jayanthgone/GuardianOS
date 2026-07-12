# GuardianOS Architecture Documentation

## 1. Overview

GuardianOS is a comprehensive Windows security system with multiple layers:

```
┌─────────────────┐
│  Mobile App     │  (Flutter)
│  (iOS/Android)  │
└────────┬────────┘
         │
    ┌────┴────┐
    │ SignalR  │
    └────┬────┘
         │
┌────────▼──────────┐
│   Backend API     │  (ASP.NET Core)
│  (JWT + SignalR)  │
├───────────────────┤
│   PostgreSQL DB   │
└─────────┬─────────┘
          │
┌─────────▼──────────┐      ┌──────────────┐
│  Windows Service   │      │ Desktop App  │
│  (Process Monitor) │      │ (WPF + MVVM) │
└────────────────────┘      └──────────────┘
          │                         │
          └────────────┬────────────┘
                       │
          ┌────────────▼──────────┐
          │   Web Dashboard       │
          │  (React + TypeScript) │
          └───────────────────────┘
```

## 2. Project Structure

### Guardian.Shared
- Shared DTOs and models used across all projects
- No business logic - pure data transfer objects

### Guardian.Common
- Common utilities and services
- Password hashing (Argon2)
- Encryption service (AES)
- Logging extensions

### Guardian.Database
- Entity Framework Core models
- Database context and migrations
- Relationship configurations

### Guardian.API
- ASP.NET Core REST API
- SignalR hubs for real-time updates
- JWT authentication
- API controllers

### Guardian.Service
- Windows background service
- Process monitoring
- Command receiver
- Runs with system privileges

### Guardian.Desktop
- WPF desktop application
- MVVM architecture
- Dashboard and management UI

### Guardian.Mobile
- Flutter mobile application
- iOS and Android support

### Guardian.Web
- React web dashboard
- TypeScript for type safety
- Tailwind CSS for styling

## 3. Security Architecture

### Authentication Flow
1. User logs in with email/password
2. Backend validates credentials
3. Backend issues JWT access token (1 hour)
4. Backend issues refresh token (7 days)
5. Client stores tokens securely
6. Client includes JWT in subsequent requests

### Process Blocking Flow
1. Windows Service monitors running processes
2. If blocked app detected:
   - Service terminates process
   - Service sends notification to API
   - API stores audit event
   - API sends SignalR notification to admin
3. Admin receives real-time notification on mobile/web
4. Admin can approve or keep blocked

## 4. Database Design

### Key Tables
- **Users**: Administrator accounts
- **Devices**: Protected computers
- **BlockedApplications**: App blocking rules
- **AuditEvents**: Security event log
- **RemoteCommands**: Tracked remote executions
- **Sessions**: Active authentication sessions

## 5. Deployment Architecture

### Development
- Local PostgreSQL database
- Local ASP.NET Core API
- Local Windows Service

### Production
- Cloud PostgreSQL (AWS RDS, Azure Database)
- Cloud API (Azure App Service, AWS EC2)
- Windows Service installed on each protected PC
- Cloud-hosted web dashboard

## 6. Code Quality Standards

- ✅ SOLID principles
- ✅ Dependency injection
- ✅ Async/await patterns
- ✅ XML documentation
- ✅ Unit tests
- ✅ Structured logging
- ✅ Configuration-driven
- ✅ Separation of concerns

## 7. Security Best Practices

- ✅ No hardcoded secrets
- ✅ HTTPS/TLS for all communication
- ✅ Input validation on all endpoints
- ✅ SQL injection prevention (EF Core parameterized queries)
- ✅ XSS prevention (React auto-escaping)
- ✅ CSRF tokens for state-changing operations
- ✅ Rate limiting on API endpoints
- ✅ Encrypted sensitive data at rest
- ✅ Audit logging for all security events
- ✅ Principle of least privilege
