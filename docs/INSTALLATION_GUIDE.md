# INES-Ruhengeri ERP System - Installation Guide

This guide provides step-by-step instructions for installing the INES-Ruhengeri ERP system.

## Table of Contents

1. [System Requirements](#system-requirements)
2. [Pre-Installation Checklist](#pre-installation-checklist)
3. [Database Setup](#database-setup)
4. [Application Installation](#application-installation)
5. [Configuration](#configuration)
6. [First Run Setup](#first-run-setup)
7. [Verification](#verification)
8. [Troubleshooting](#troubleshooting)

## System Requirements

### Minimum Requirements

- **Operating System**: Windows 10 (64-bit) or Windows Server 2019
- **Processor**: Intel Core i3 or AMD equivalent
- **Memory**: 4 GB RAM
- **Storage**: 10 GB available disk space
- **Network**: Internet connection for initial setup
- **Display**: 1366x768 resolution

### Recommended Requirements

- **Operating System**: Windows 11 (64-bit) or Windows Server 2022
- **Processor**: Intel Core i5 or AMD equivalent
- **Memory**: 8 GB RAM or higher
- **Storage**: 50 GB available disk space (SSD recommended)
- **Network**: Stable broadband internet connection
- **Display**: 1920x1080 resolution or higher

### Software Dependencies

- **.NET 8.0 Runtime**: Required for application execution
- **SQL Server**: 2019 Express or higher (or PostgreSQL 12+)
- **Visual C++ Redistributable**: Latest version

## Pre-Installation Checklist

### Administrator Privileges

Ensure you have administrator privileges on the target machine for:
- Installing software components
- Creating database connections
- Configuring Windows services (if applicable)
- Modifying system settings

### Network Configuration

- Ensure firewall allows database connections (port 1433 for SQL Server)
- Configure antivirus exclusions for application folder
- Verify internet connectivity for license validation

### Backup Considerations

- Backup existing data if upgrading from previous version
- Document current system configuration
- Create system restore point

## Database Setup

### Option 1: SQL Server Installation

1. **Download SQL Server**
   ```
   Download SQL Server 2019 Express from:
   https://www.microsoft.com/en-us/sql-server/sql-server-downloads
   ```

2. **Install SQL Server**
   - Run the installer as administrator
   - Choose "Basic" installation type
   - Accept license terms
   - Complete installation

3. **Install SQL Server Management Studio (SSMS)**
   ```
   Download SSMS from:
   https://docs.microsoft.com/en-us/sql/ssms/download-sql-server-management-studio-ssms
   ```

4. **Create Database**
   ```sql
   -- Connect to SQL Server using SSMS
   -- Create new database
   CREATE DATABASE INES_ERP
   GO
   
   -- Create application user
   CREATE LOGIN ines_erp_user WITH PASSWORD = 'YourSecurePassword123!'
   GO
   
   USE INES_ERP
   GO
   
   CREATE USER ines_erp_user FOR LOGIN ines_erp_user
   GO
   
   -- Grant permissions
   ALTER ROLE db_datareader ADD MEMBER ines_erp_user
   ALTER ROLE db_datawriter ADD MEMBER ines_erp_user
   ALTER ROLE db_ddladmin ADD MEMBER ines_erp_user
   GO
   ```

### Option 2: PostgreSQL Installation

1. **Download PostgreSQL**
   ```
   Download PostgreSQL from:
   https://www.postgresql.org/download/windows/
   ```

2. **Install PostgreSQL**
   - Run installer as administrator
   - Set password for postgres user
   - Note the port number (default: 5432)
   - Complete installation

3. **Create Database**
   ```sql
   -- Connect using pgAdmin or psql
   CREATE DATABASE ines_erp;
   CREATE USER ines_erp_user WITH PASSWORD 'YourSecurePassword123!';
   GRANT ALL PRIVILEGES ON DATABASE ines_erp TO ines_erp_user;
   ```

## Application Installation

### Method 1: MSI Installer (Recommended)

1. **Download Installer**
   - Download `INES-ERP-Setup.msi` from the official source
   - Verify file integrity using provided checksums

2. **Run Installation**
   ```
   Right-click INES-ERP-Setup.msi → "Run as administrator"
   ```

3. **Installation Wizard**
   - Accept license agreement
   - Choose installation directory (default: `C:\Program Files\INES-ERP\`)
   - Select components to install
   - Configure database connection
   - Complete installation

### Method 2: Portable Installation

1. **Download Portable Package**
   - Download `INES-ERP-Portable.zip`
   - Extract to desired location (e.g., `C:\INES-ERP\`)

2. **Install .NET Runtime**
   ```
   Download .NET 8.0 Runtime from:
   https://dotnet.microsoft.com/download/dotnet/8.0
   ```

3. **Set Permissions**
   - Ensure application folder has write permissions
   - Configure Windows Defender exclusions

## Configuration

### Database Connection

1. **Locate Configuration File**
   ```
   Installation Directory\appsettings.json
   ```

2. **Update Connection String**
   ```json
   {
     "ConnectionStrings": {
       "DefaultConnection": "Server=localhost;Database=INES_ERP;User Id=ines_erp_user;Password=YourSecurePassword123!;Encrypt=true;TrustServerCertificate=true;"
     }
   }
   ```

### Application Settings

1. **Basic Configuration**
   ```json
   {
     "ApplicationSettings": {
       "OrganizationName": "INES-Ruhengeri",
       "Currency": "RWF",
       "TimeZone": "Africa/Kigali",
       "Language": "en-US"
     }
   }
   ```

2. **Security Settings**
   ```json
   {
     "Security": {
       "PasswordMinLength": 8,
       "RequireDigit": true,
       "RequireLowercase": true,
       "RequireUppercase": true,
       "RequireNonAlphanumeric": true,
       "SessionTimeoutMinutes": 60
     }
   }
   ```

### Email Configuration (Optional)

```json
{
  "EmailSettings": {
    "SmtpHost": "smtp.gmail.com",
    "SmtpPort": 587,
    "EnableSsl": true,
    "Username": "your-email@gmail.com",
    "Password": "your-app-password",
    "FromEmail": "noreply@ines.ac.rw",
    "FromName": "INES ERP System"
  }
}
```

## First Run Setup

### Initial Database Migration

1. **Open Command Prompt as Administrator**
   ```cmd
   cd "C:\Program Files\INES-ERP"
   ```

2. **Run Database Migration**
   ```cmd
   INES.ERP.WPF.exe --migrate-database
   ```

3. **Verify Migration**
   - Check for success message
   - Verify tables created in database

### Create Administrator Account

1. **Launch Application**
   ```
   Double-click INES.ERP.WPF.exe
   ```

2. **Initial Setup Wizard**
   - Enter organization details
   - Create administrator account:
     - Username: admin
     - Password: (secure password)
     - Email: admin@ines.ac.rw
   - Configure basic settings

3. **Complete Setup**
   - Review configuration
   - Finalize setup
   - Login with administrator account

## Verification

### System Health Check

1. **Database Connectivity**
   - Login to application
   - Navigate to System → Database Status
   - Verify "Connected" status

2. **Core Modules**
   - Test student management
   - Create sample billing record
   - Process test payment
   - Generate basic report

3. **User Management**
   - Create test user account
   - Assign permissions
   - Test login with new account

### Performance Verification

1. **Response Times**
   - Dashboard load time < 3 seconds
   - Report generation < 10 seconds
   - Data entry forms < 1 second

2. **Memory Usage**
   - Initial load < 200 MB
   - Normal operation < 500 MB
   - Check for memory leaks

## Troubleshooting

### Common Installation Issues

1. **"Database Connection Failed"**
   ```
   Solutions:
   - Verify SQL Server is running
   - Check connection string
   - Ensure user has proper permissions
   - Test connection using SSMS
   ```

2. **".NET Runtime Not Found"**
   ```
   Solutions:
   - Download and install .NET 8.0 Runtime
   - Restart computer after installation
   - Verify installation: dotnet --version
   ```

3. **"Access Denied" Errors**
   ```
   Solutions:
   - Run as administrator
   - Check folder permissions
   - Configure antivirus exclusions
   - Disable UAC temporarily
   ```

4. **"Port Already in Use"**
   ```
   Solutions:
   - Check for other applications using port
   - Change application port in configuration
   - Restart Windows
   ```

### Database Issues

1. **Migration Failures**
   ```sql
   -- Check database permissions
   SELECT 
       dp.name AS principal_name,
       dp.type_desc AS principal_type,
       o.name AS object_name,
       p.permission_name,
       p.state_desc AS permission_state
   FROM sys.database_permissions p
   LEFT JOIN sys.objects o ON p.major_id = o.object_id
   LEFT JOIN sys.database_principals dp ON p.grantee_principal_id = dp.principal_id
   WHERE dp.name = 'ines_erp_user'
   ```

2. **Performance Issues**
   ```sql
   -- Check database size and growth
   SELECT 
       name,
       size/128.0 AS CurrentSizeMB,
       size/128.0 - CAST(FILEPROPERTY(name, 'SpaceUsed') AS INT)/128.0 AS FreeSpaceMB
   FROM sys.database_files
   ```

### Application Issues

1. **Slow Performance**
   - Check available RAM
   - Verify SSD/HDD performance
   - Review database indexes
   - Check network connectivity

2. **UI Issues**
   - Update graphics drivers
   - Check display scaling settings
   - Verify .NET Framework components
   - Clear application cache

### Getting Help

1. **Log Files**
   ```
   Location: Installation Directory\Logs\
   Files: 
   - application-YYYY-MM-DD.log
   - error-YYYY-MM-DD.log
   - database-YYYY-MM-DD.log
   ```

2. **Support Contacts**
   - Email: support@ines.ac.rw
   - Phone: +250-XXX-XXXX
   - Documentation: https://docs.ines.ac.rw/erp

3. **System Information**
   ```
   Collect the following for support:
   - Windows version and build
   - .NET runtime version
   - Database version
   - Application version
   - Error messages and logs
   - Steps to reproduce issue
   ```

---

**Note**: Keep this installation guide updated with each software release. Always test installation procedures in a staging environment before production deployment.
