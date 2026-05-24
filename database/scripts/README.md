# データベーススクリプト

本ディレクトリには **手動 DDL スクリプト** を格納し、Azure SQL Database / SQL Server スキーマの初期化と更新に使用する。

## 原則

- **EF Core Migration は使用しない**。`AppDbContext` は実行時 ORM マッピングのみ。テーブル構造は SQL スクリプトで管理する。
- スクリプトは連番で命名：`001_initial.sql`、`002_xxx.sql` …
- 各スクリプトは再実行可能（`IF NOT EXISTS` / `IF OBJECT_ID IS NULL` 等の冪等記述）。

## 使用方法

1. SSMS または Azure Data Studio で対象 DB に接続する。
2. 連番順にスクリプトを実行する。新環境では先に `001_initial.sql` を実行する。
3. 接続文字列は `src/Api/appsettings.Development.json` の `ConnectionStrings:DefaultConnection` を参照。

## EF エンティティとの対応

| 表名 | EF エンティティ |
|------|---------|
| DemoItems | `Infrastructure.Entities.DemoItem` |
| Vouchers | `Infrastructure.Entities.Voucher` |
| ProcessedBlobRecords | `Infrastructure.Entities.ProcessedBlobRecord` |
| ApplicationTypes | `Infrastructure.Entities.ApplicationType` |
| ApplicationTypeFields | `Infrastructure.Entities.ApplicationTypeField` |
| CsvColumnMappings | `Infrastructure.Entities.CsvColumnMapping` |
| Departments | `Infrastructure.Entities.Department` |
| UserRoles | `Infrastructure.Entities.UserRole` |

## シードデータ

開発環境で API 初回起動時、`AuthDataSeeder` は DB が空の場合に既定科室 `DEV`（および任意の開発ユーザーロール）を挿入する。業務データはアプリまたは別 DML スクリプトで管理する。
