# GuardianOS - Enterprise Windows Security & Remote Administration System

A production-quality Windows desktop security system that allows administrators to remotely control and protect their Windows PCs from desktop, web, and mobile applications.

## 🏗️ Architecture Overview

```
GuardianOS/
├── Guardian.Desktop/          # WPF Desktop Application
├── Guardian.Service/          # Windows Background Service
├── Guardian.API/              # ASP.NET Core Web API
├── Guardian.Mobile/           # Flutter Mobile App
├── Guardian.Web/              # React Web Dashboard
├── Guardian.Shared/           # Shared DTOs & Models
├── Guardian.Common/           # Common Utilities & Helpers
└── Guardian.Database/         # Entity Framework Core Models
```

## 🛠️ Tech Stack

### Desktop
- **Language**: C#
- **Framework**: .NET 9
- **UI**: WPF with Fluent Design
- **Architecture**: MVVM with CommunityToolkit.MVVM

### Backend
- **Language**: C#
- **Framework**: ASP.NET Core 9
- **Real-time**: SignalR
- **Database**: PostgreSQL with Entity Framework Core
- **Authentication**: JWT with Refresh Tokens

### Frontend
- **Mobile**: Flutter
- **Web**: React + TypeScript + Vite + Tailwind CSS

## 🔐 Security Features

✅ Secure Administrator Authentication
✅ JWT Token-based Authorization
✅ Device Registration & Session Management
✅ Process Monitoring & Application Blocking
✅ Password-Protected Application Access
✅ Encrypted Offline Policies
✅ Comprehensive Audit Logging
✅ HTTPS/TLS Communication
✅ Argon2/BCrypt Password Hashing
✅ SQL Injection & XSS Prevention

## 📚 Getting Started

### Prerequisites
- .NET 9 SDK
- PostgreSQL 14+
- Node.js 18+
- Flutter SDK
- Visual Studio 2022 or JetBrains Rider

### Setup

1. Clone the repository
2. Configure database connection in `appsettings.json`
3. Run Entity Framework migrations
4. Build desktop application
5. Start backend API
6. Deploy Windows Service
7. Configure mobile and web applications

## 📖 Documentation

See [ARCHITECTURE.md](ARCHITECTURE.md) for detailed architecture documentation.

## 🤝 Contributing

Contributions are welcome! Please follow SOLID principles and ensure code quality standards.

## 📄 License

Proprietary - All rights reserved
