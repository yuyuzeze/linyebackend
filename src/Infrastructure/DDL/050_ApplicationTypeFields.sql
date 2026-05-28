/*
    Table: dbo.ApplicationTypeFields
    Entity: Infrastructure.Entities.ApplicationTypeField
*/
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
