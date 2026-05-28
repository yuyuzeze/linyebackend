/*
    Table: dbo.UserRoles
    Entity: Infrastructure.Entities.UserRole
    Depends on: dbo.Departments (070)
*/
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
