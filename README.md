# INES-Ruhengeri ERP System

A comprehensive, production-ready Enterprise Resource Planning (ERP) system built with C# WPF, designed specifically for educational institutions with a focus on accounting and business management.

## 🏫 About INES-Ruhengeri

This ERP system is designed for the Institut d'Enseignement Supérieur de Ruhengeri (INES-Ruhengeri) in Rwanda, providing a complete solution for managing academic finances, student accounts, inventory, payroll, and institutional operations.

## ✨ Features

### Core Modules
- **Authentication & Security** - Role-based access control, two-factor authentication, audit logging
- **Dashboard & Analytics** - Real-time KPIs, financial charts, drill-down analytics
- **Student Accounts** - Billing, payment tracking, penalty management, receipts
- **Project Accounts** - Grant management, milestone disbursements, project profitability
- **Stores & Inventory** - Multi-store management, stock valuation, reorder alerts
- **Accounting** - Double-entry bookkeeping, chart of accounts, trial balance
- **Payroll & HR** - Employee management, payslip generation, statutory deductions
- **Procurement** - Vendor management, purchase orders, goods received notes
- **Fixed Assets** - Asset registry, depreciation, QR/barcode tagging
- **Financial Reporting** - Income statements, balance sheets, cash flow statements
- **Advanced Analytics** - Custom report builder, audit trails, compliance reports

### Technical Features
- **Modern UI** - Material Design with light/dark themes
- **Responsive Design** - Adaptive layouts for different screen sizes
- **Performance Optimized** - Lazy loading, background threading, async operations
- **Secure** - BCrypt password hashing, session management, audit trails
- **Scalable Architecture** - MVVM pattern, dependency injection, modular design
- **Database Support** - SQL Server and PostgreSQL with Entity Framework Core
- **Export Capabilities** - PDF, Excel, CSV export functionality

## 🛠️ Technology Stack

- **Framework**: .NET 8.0
- **UI Framework**: WPF (Windows Presentation Foundation)
- **Architecture**: MVVM (Model-View-ViewModel)
- **Database**: SQL Server / PostgreSQL
- **ORM**: Entity Framework Core 8.0
- **UI Library**: Material Design in XAML
- **Charts**: LiveCharts2
- **Dependency Injection**: Microsoft.Extensions.DependencyInjection
- **Logging**: Serilog
- **Testing**: xUnit
- **Security**: BCrypt.Net

## 🚀 Getting Started

### Prerequisites
- Visual Studio 2022 or later
- .NET 8.0 SDK
- SQL Server 2019+ or PostgreSQL 12+
- Windows 10/11

### Installation

1. **Clone the repository**
   ```bash
   git clone https://github.com/your-org/ines-erp-system.git
   cd ines-erp-system
   ```

2. **Restore NuGet packages**
   ```bash
   dotnet restore
   ```

3. **Configure database connection**
   - Update `appsettings.json` with your database connection string
   - For SQL Server (default):
     ```json
     "ConnectionStrings": {
       "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=INES_ERP;Trusted_Connection=true;MultipleActiveResultSets=true"
     }
     ```

4. **Run database migrations**
   ```bash
   dotnet ef database update --project src/INES.ERP.Data
   ```

5. **Build and run**
   ```bash
   dotnet build
   dotnet run --project src/INES.ERP.WPF
   ```

### Default Login Credentials
- **Username**: `admin`
- **Password**: `Admin@123`
- **Role**: Administrator

> ⚠️ **Important**: Change the default password immediately after first login in production environments.

## 📁 Project Structure

```
INES.ERP/
├── src/
│   ├── INES.ERP.WPF/          # WPF Application (Presentation Layer)
│   │   ├── Views/             # XAML Views
│   │   ├── ViewModels/        # View Models
│   │   ├── Controls/          # Custom Controls
│   │   ├── Converters/        # Value Converters
│   │   ├── Styles/            # XAML Styles
│   │   └── Resources/         # Resources and Assets
│   ├── INES.ERP.Core/         # Domain Models and Interfaces
│   │   ├── Models/            # Entity Models
│   │   ├── Interfaces/        # Service and Repository Interfaces
│   │   ├── Enums/             # Enumerations
│   │   └── Constants/         # Application Constants
│   ├── INES.ERP.Data/         # Data Access Layer
│   │   ├── Repositories/      # Repository Implementations
│   │   ├── Configurations/    # Entity Configurations
│   │   └── Migrations/        # Database Migrations
│   └── INES.ERP.Services/     # Business Logic Layer
│       ├── Authentication/    # Authentication Services
│       ├── StudentAccounts/   # Student Account Services
│       ├── Accounting/        # Accounting Services
│       └── Common/            # Common Services
├── tests/
│   └── INES.ERP.Tests/        # Unit and Integration Tests
└── docs/                      # Documentation
```

## 🔧 Configuration

### Database Configuration
The application supports both SQL Server and PostgreSQL. Configure your preferred database in `appsettings.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=INES_ERP;Trusted_Connection=true;",
    "PostgreSQL": "Host=localhost;Database=ines_erp;Username=postgres;Password=password"
  }
}
```

### Application Settings
Key configuration options in `appsettings.json`:

- **Authentication**: Token expiry, password policies, two-factor settings
- **Security**: Session timeout, password requirements
- **Features**: Enable/disable specific modules
- **Performance**: Caching, database timeouts
- **Localization**: Supported languages and cultures

## 👥 User Roles

The system supports multiple user roles with different access levels:

- **Administrator** - Full system access
- **Bursar** - Financial management and accounting
- **Store Manager** - Inventory and procurement management
- **Auditor** - Read-only access for audit purposes
- **Accountant** - Accounting module access
- **HR Manager** - Payroll and employee management
- **Project Manager** - Project accounts management
- **Student Affairs** - Student accounts management

## 📊 Compliance

The system is designed to comply with:
- **IFRS** (International Financial Reporting Standards)
- **RRA** (Rwanda Revenue Authority) requirements
- **Educational Institution** financial regulations
- **Data Protection** and privacy standards

## 🧪 Testing

Run the test suite:
```bash
dotnet test
```

The project includes:
- Unit tests for business logic
- Integration tests for data access
- UI automation tests (planned)

## 📖 Documentation

- [User Manual](docs/user-manual.md)
- [Administrator Guide](docs/admin-guide.md)
- [Developer Documentation](docs/developer-guide.md)
- [API Reference](docs/api-reference.md)

## 🤝 Contributing

1. Fork the repository
2. Create a feature branch (`git checkout -b feature/amazing-feature`)
3. Commit your changes (`git commit -m 'Add amazing feature'`)
4. Push to the branch (`git push origin feature/amazing-feature`)
5. Open a Pull Request

## 📄 License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

## 🆘 Support

For support and questions:
- Email: support@ines.ac.rw
- Documentation: [Wiki](https://github.com/your-org/ines-erp-system/wiki)
- Issues: [GitHub Issues](https://github.com/your-org/ines-erp-system/issues)

## 🙏 Acknowledgments

- INES-Ruhengeri for project requirements and testing
- Material Design in XAML community
- .NET and WPF development community
- All contributors and testers

---

**Built with ❤️ for INES-Ruhengeri**
