/*
    Table: dbo.DemoItems
    Entity: Infrastructure.Entities.DemoItem
*/
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
