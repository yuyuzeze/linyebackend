/*
    Table: dbo.ApplicationTypes
    Entity: Infrastructure.Entities.ApplicationType
*/
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
