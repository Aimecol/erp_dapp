# INES-Ruhengeri ERP System - Deployment Guide

This guide provides comprehensive instructions for deploying the INES-Ruhengeri ERP system in various environments.

## Table of Contents

1. [Prerequisites](#prerequisites)
2. [Environment Setup](#environment-setup)
3. [Database Setup](#database-setup)
4. [Application Configuration](#application-configuration)
5. [Deployment Options](#deployment-options)
6. [Security Configuration](#security-configuration)
7. [Monitoring and Logging](#monitoring-and-logging)
8. [Backup and Recovery](#backup-and-recovery)
9. [Troubleshooting](#troubleshooting)

## Prerequisites

### System Requirements

- **Operating System**: Windows 10/11, Windows Server 2019/2022
- **Framework**: .NET 8.0 Runtime
- **Database**: SQL Server 2019+ or PostgreSQL 12+
- **Memory**: Minimum 4GB RAM (8GB recommended)
- **Storage**: Minimum 10GB free space (50GB recommended)
- **Network**: Internet connection for initial setup and updates

### Software Dependencies

- .NET 8.0 Runtime
- SQL Server or PostgreSQL
- IIS (for web deployment) or Windows Service hosting
- Visual C++ Redistributable (latest)

## Environment Setup

### Development Environment

1. **Install .NET 8.0 SDK**
   ```bash
   # Download from https://dotnet.microsoft.com/download/dotnet/8.0
   # Verify installation
   dotnet --version
   ```

2. **Install Database Server**
   - **SQL Server**: Download SQL Server Express or Developer Edition
   - **PostgreSQL**: Download from https://www.postgresql.org/download/

3. **Clone Repository**
   ```bash
   git clone https://github.com/your-org/ines-erp-system.git
   cd ines-erp-system
   ```

4. **Restore Dependencies**
   ```bash
   dotnet restore
   ```

### Production Environment

1. **Server Preparation**
   - Ensure Windows Server is updated
   - Install .NET 8.0 Runtime (not SDK)
   - Configure Windows Firewall
   - Set up antivirus exclusions

2. **Database Server Setup**
   - Install and configure SQL Server or PostgreSQL
   - Create dedicated database user
   - Configure backup strategy
   - Set up monitoring

## Database Setup

### SQL Server Setup

1. **Create Database**
   ```sql
   CREATE DATABASE INES_ERP
   GO
   
   USE INES_ERP
   GO
   ```

2. **Create Application User**
   ```sql
   CREATE LOGIN ines_erp_user WITH PASSWORD = 'YourSecurePassword123!'
   GO
   
   USE INES_ERP
   GO
   
   CREATE USER ines_erp_user FOR LOGIN ines_erp_user
   GO
   
   ALTER ROLE db_datareader ADD MEMBER ines_erp_user
   ALTER ROLE db_datawriter ADD MEMBER ines_erp_user
   ALTER ROLE db_ddladmin ADD MEMBER ines_erp_user
   GO
   ```

3. **Run Migrations**
   ```bash
   dotnet ef database update --project src/INES.ERP.Data --startup-project src/INES.ERP.WPF
   ```

### PostgreSQL Setup

1. **Create Database and User**
   ```sql
   CREATE DATABASE ines_erp;
   CREATE USER ines_erp_user WITH PASSWORD 'YourSecurePassword123!';
   GRANT ALL PRIVILEGES ON DATABASE ines_erp TO ines_erp_user;
   ```

2. **Configure Connection String**
   ```json
   {
     "ConnectionStrings": {
       "DefaultConnection": "Host=localhost;Database=ines_erp;Username=ines_erp_user;Password=YourSecurePassword123!"
     }
   }
   ```

## Application Configuration

### Configuration Files

1. **appsettings.json** (Base configuration)
   ```json
   {
     "ConnectionStrings": {
       "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=INES_ERP;Trusted_Connection=true;MultipleActiveResultSets=true"
     },
     "Authentication": {
       "TokenExpiryMinutes": 60,
       "RefreshTokenExpiryDays": 30,
       "MaxFailedAttempts": 5,
       "LockoutDurationMinutes": 15
     },
     "Security": {
       "PasswordMinLength": 8,
       "RequireDigit": true,
       "RequireLowercase": true,
       "RequireUppercase": true,
       "RequireNonAlphanumeric": true
     },
     "Features": {
       "EnableTwoFactorAuth": true,
       "EnableAuditLogging": true,
       "EnableEmailNotifications": false
     },
     "Logging": {
       "LogLevel": {
         "Default": "Information",
         "Microsoft.AspNetCore": "Warning"
       }
     }
   }
   ```

2. **appsettings.Production.json** (Production overrides)
   ```json
   {
     "ConnectionStrings": {
       "DefaultConnection": "Server=prod-sql-server;Database=INES_ERP;User Id=ines_erp_user;Password=YourSecurePassword123!;Encrypt=true;TrustServerCertificate=false;"
     },
     "Logging": {
       "LogLevel": {
         "Default": "Warning",
         "INES.ERP": "Information"
       }
     }
   }
   ```

### Environment Variables

Set the following environment variables for production:

```bash
# Database
INES_ERP_DB_CONNECTION="Server=prod-sql-server;Database=INES_ERP;User Id=ines_erp_user;Password=YourSecurePassword123!;"

# Security
INES_ERP_JWT_SECRET="YourJWTSecretKey256BitsLong"
INES_ERP_ENCRYPTION_KEY="YourEncryptionKey256BitsLong"

# Email (if enabled)
INES_ERP_SMTP_HOST="smtp.gmail.com"
INES_ERP_SMTP_PORT="587"
INES_ERP_SMTP_USERNAME="your-email@gmail.com"
INES_ERP_SMTP_PASSWORD="your-app-password"

# Environment
ASPNETCORE_ENVIRONMENT="Production"
```

## Deployment Options

### Option 1: Standalone Desktop Application

1. **Publish Application**
   ```bash
   dotnet publish src/INES.ERP.WPF -c Release -r win-x64 --self-contained true -o ./publish
   ```

2. **Create Installer** (Optional)
   - Use tools like Inno Setup or WiX Toolset
   - Include .NET Runtime if not self-contained
   - Configure auto-updater

3. **Distribution**
   - Copy published files to target machines
   - Run setup.exe or copy application folder
   - Configure database connection

### Option 2: Windows Service

1. **Modify for Service Hosting**
   ```csharp
   // In Program.cs
   public static void Main(string[] args)
   {
       if (args.Length > 0 && args[0] == "--service")
       {
           RunAsService();
       }
       else
       {
           RunAsApplication();
       }
   }
   ```

2. **Install as Service**
   ```bash
   sc create "INES ERP Service" binPath="C:\INES-ERP\INES.ERP.WPF.exe --service"
   sc start "INES ERP Service"
   ```

### Option 3: Network Deployment

1. **Shared Network Location**
   - Deploy to shared network drive
   - Configure ClickOnce deployment
   - Set up automatic updates

2. **Group Policy Deployment**
   - Create MSI package
   - Deploy via Group Policy
   - Configure automatic updates

## Security Configuration

### Database Security

1. **Connection Security**
   - Use encrypted connections (SSL/TLS)
   - Implement connection pooling
   - Use least privilege principle

2. **Data Encryption**
   - Encrypt sensitive data at rest
   - Use column-level encryption for PII
   - Implement transparent data encryption

### Application Security

1. **Authentication**
   - Configure strong password policies
   - Enable two-factor authentication
   - Implement account lockout policies

2. **Authorization**
   - Use role-based access control
   - Implement principle of least privilege
   - Regular permission audits

3. **Data Protection**
   - Encrypt configuration files
   - Secure API endpoints
   - Implement audit logging

### Network Security

1. **Firewall Configuration**
   ```bash
   # Allow SQL Server (default port)
   netsh advfirewall firewall add rule name="SQL Server" dir=in action=allow protocol=TCP localport=1433
   
   # Allow PostgreSQL (default port)
   netsh advfirewall firewall add rule name="PostgreSQL" dir=in action=allow protocol=TCP localport=5432
   ```

2. **SSL/TLS Configuration**
   - Use valid SSL certificates
   - Configure minimum TLS version
   - Disable weak cipher suites

## Monitoring and Logging

### Application Monitoring

1. **Performance Counters**
   - CPU usage
   - Memory consumption
   - Database connections
   - Response times

2. **Health Checks**
   - Database connectivity
   - External service availability
   - Disk space monitoring

### Logging Configuration

1. **Serilog Setup**
   ```json
   {
     "Serilog": {
       "Using": ["Serilog.Sinks.File", "Serilog.Sinks.Console"],
       "MinimumLevel": "Information",
       "WriteTo": [
         {
           "Name": "File",
           "Args": {
             "path": "logs/ines-erp-.txt",
             "rollingInterval": "Day",
             "retainedFileCountLimit": 30
           }
         }
       ]
     }
   }
   ```

2. **Log Management**
   - Implement log rotation
   - Set up log aggregation
   - Configure alerting

## Backup and Recovery

### Database Backup

1. **Automated Backups**
   ```sql
   -- SQL Server backup script
   BACKUP DATABASE INES_ERP 
   TO DISK = 'C:\Backups\INES_ERP_Full.bak'
   WITH FORMAT, INIT, COMPRESSION;
   ```

2. **Backup Schedule**
   - Full backup: Weekly
   - Differential backup: Daily
   - Transaction log backup: Every 15 minutes

### Application Backup

1. **Configuration Backup**
   - Backup configuration files
   - Export system settings
   - Document customizations

2. **Data Export**
   - Regular data exports
   - Report templates backup
   - User preferences backup

## Troubleshooting

### Common Issues

1. **Database Connection Issues**
   ```bash
   # Test connection
   sqlcmd -S server_name -U username -P password -Q "SELECT 1"
   
   # Check SQL Server services
   net start | findstr SQL
   ```

2. **Permission Issues**
   - Verify user permissions
   - Check file system permissions
   - Validate database permissions

3. **Performance Issues**
   - Monitor resource usage
   - Check database indexes
   - Review query performance

### Diagnostic Tools

1. **Event Viewer**
   - Application logs
   - System logs
   - Security logs

2. **Performance Monitor**
   - CPU usage
   - Memory usage
   - Disk I/O

3. **Database Tools**
   - SQL Server Management Studio
   - pgAdmin (PostgreSQL)
   - Database performance monitoring

### Support Contacts

- **Technical Support**: support@ines.ac.rw
- **Emergency Contact**: +250-XXX-XXXX
- **Documentation**: https://github.com/your-org/ines-erp-system/wiki

---

**Note**: This deployment guide should be customized based on your specific infrastructure and requirements. Always test deployments in a staging environment before production deployment.
