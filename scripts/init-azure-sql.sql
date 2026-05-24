/*
    Manual schema initialization for Azure SQL Database.
    Execute this script in SSMS against your target database.

    Schema changes are managed by DDL scripts (not EF migrations).
    For incremental updates, add new scripts under scripts/ddl/.
*/

SET ANSI_NULLS ON;
SET QUOTED_IDENTIFIER ON;
GO

IF OBJECT_ID(N'dbo.DemoItems', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.DemoItems
    (
        Id INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
        Name NVARCHAR(200) NOT NULL,
        Description NVARCHAR(2000) NULL,
        CreatedAt DATETIME2 NOT NULL
    );
END;
GO

IF OBJECT_ID(N'dbo.Vouchers', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.Vouchers
    (
        Id INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
        VoucherDate DATETIME2 NOT NULL,
        Summary NVARCHAR(500) NOT NULL,
        DebitAccount NVARCHAR(100) NULL,
        CreditAccount NVARCHAR(100) NULL,
        Amount DECIMAL(18,2) NOT NULL,
        SourceBlobPath NVARCHAR(500) NULL,
        CreatedAt DATETIME2 NOT NULL
    );
END;
GO

IF OBJECT_ID(N'dbo.ProcessedBlobRecords', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.ProcessedBlobRecords
    (
        Id INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
        ContainerName NVARCHAR(200) NOT NULL,
        BlobName NVARCHAR(500) NOT NULL,
        BlobETag NVARCHAR(100) NULL,
        ProcessedAt DATETIME2 NOT NULL,
        Status NVARCHAR(20) NOT NULL,
        ErrorMessage NVARCHAR(2000) NULL,
        RowCount INT NULL
    );
END;
GO

IF COL_LENGTH(N'dbo.ProcessedBlobRecords', N'RowCount') IS NULL
BEGIN
    ALTER TABLE dbo.ProcessedBlobRecords ADD RowCount INT NULL;
END;
GO

IF NOT EXISTS (
    SELECT 1
    FROM sys.indexes
    WHERE name = N'IX_ProcessedBlobRecords_ContainerName_BlobName'
      AND object_id = OBJECT_ID(N'dbo.ProcessedBlobRecords')
)
BEGIN
    CREATE UNIQUE INDEX IX_ProcessedBlobRecords_ContainerName_BlobName
        ON dbo.ProcessedBlobRecords (ContainerName, BlobName);
END;
GO

IF OBJECT_ID(N'dbo.ApplicationTypes', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.ApplicationTypes
    (
        Id INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
        Code NVARCHAR(50) NOT NULL,
        Name NVARCHAR(200) NOT NULL,
        Description NVARCHAR(500) NULL,
        DisplayOrder INT NOT NULL
    );
END;
GO

IF NOT EXISTS (
    SELECT 1
    FROM sys.indexes
    WHERE name = N'IX_ApplicationTypes_Code'
      AND object_id = OBJECT_ID(N'dbo.ApplicationTypes')
)
BEGIN
    CREATE UNIQUE INDEX IX_ApplicationTypes_Code
        ON dbo.ApplicationTypes (Code);
END;
GO

IF OBJECT_ID(N'dbo.ApplicationTypeFields', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.ApplicationTypeFields
    (
        Id INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
        ApplicationTypeId INT NOT NULL,
        FieldCode NVARCHAR(100) NOT NULL,
        FieldName NVARCHAR(200) NOT NULL,
        DataType NVARCHAR(50) NOT NULL,
        DisplayOrder INT NOT NULL,
        IsRequired BIT NOT NULL
    );
END;
GO

IF NOT EXISTS (
    SELECT 1
    FROM sys.indexes
    WHERE name = N'IX_ApplicationTypeFields_ApplicationTypeId_FieldCode'
      AND object_id = OBJECT_ID(N'dbo.ApplicationTypeFields')
)
BEGIN
    CREATE UNIQUE INDEX IX_ApplicationTypeFields_ApplicationTypeId_FieldCode
        ON dbo.ApplicationTypeFields (ApplicationTypeId, FieldCode);
END;
GO

IF OBJECT_ID(N'dbo.CsvColumnMappings', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.CsvColumnMappings
    (
        Id INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
        ApplicationTypeId INT NOT NULL,
        CsvColumnIndex INT NOT NULL,
        CsvColumnName NVARCHAR(200) NULL,
        TargetFieldCode NVARCHAR(100) NOT NULL
    );
END;
GO

IF OBJECT_ID(N'dbo.Departments', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.Departments
    (
        Id INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
        Code NVARCHAR(50) NOT NULL,
        Name NVARCHAR(200) NOT NULL
    );
END;
GO

IF NOT EXISTS (
    SELECT 1
    FROM sys.indexes
    WHERE name = N'IX_Departments_Code'
      AND object_id = OBJECT_ID(N'dbo.Departments')
)
BEGIN
    CREATE UNIQUE INDEX IX_Departments_Code
        ON dbo.Departments (Code);
END;
GO

IF OBJECT_ID(N'dbo.UserRoles', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.UserRoles
    (
        Id INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
        EntraObjectId NVARCHAR(64) NOT NULL,
        Upn NVARCHAR(256) NOT NULL,
        DepartmentId INT NULL,
        RoleCode NVARCHAR(50) NOT NULL,
        IsActive BIT NOT NULL,
        CONSTRAINT FK_UserRoles_Departments_DepartmentId
            FOREIGN KEY (DepartmentId) REFERENCES dbo.Departments (Id)
            ON DELETE SET NULL
    );
END;
GO

IF NOT EXISTS (
    SELECT 1
    FROM sys.indexes
    WHERE name = N'IX_UserRoles_DepartmentId'
      AND object_id = OBJECT_ID(N'dbo.UserRoles')
)
BEGIN
    CREATE INDEX IX_UserRoles_DepartmentId
        ON dbo.UserRoles (DepartmentId);
END;
GO

IF NOT EXISTS (
    SELECT 1
    FROM sys.indexes
    WHERE name = N'IX_UserRoles_EntraObjectId'
      AND object_id = OBJECT_ID(N'dbo.UserRoles')
)
BEGIN
    CREATE INDEX IX_UserRoles_EntraObjectId
        ON dbo.UserRoles (EntraObjectId);
END;
GO
