/*
    Table: dbo.CsvColumnMappings
    Entity: Infrastructure.Entities.CsvColumnMapping
*/
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
