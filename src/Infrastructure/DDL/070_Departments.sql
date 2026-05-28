/*
    Table: dbo.Departments
    Entity: Infrastructure.Entities.Department
*/
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
