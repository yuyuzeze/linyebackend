/*
    Table: dbo.Vouchers
    Entity: Infrastructure.Entities.Voucher
*/
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
