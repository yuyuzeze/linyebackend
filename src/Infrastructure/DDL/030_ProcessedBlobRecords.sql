/*
    Table: dbo.ProcessedBlobRecords
    Entity: Infrastructure.Entities.ProcessedBlobRecord
*/
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
