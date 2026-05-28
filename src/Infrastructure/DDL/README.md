# DDL 脚本（Infrastructure）

本目录为 **Azure SQL / SQL Server** 表结构的唯一维护位置（不使用 EF Core Migration）。

## 原则

- `AppDbContext` 仅做运行时 ORM 映射，建表/改表由此处 SQL 管理。
- 每个脚本可重复执行（`IF NOT EXISTS` / `IF OBJECT_ID IS NULL` 等幂等写法）。
- 按 **序号前缀** 控制执行顺序；有外键依赖时序号小的先执行。

## 初始建表（按顺序执行）

| 脚本 | 表 |
|------|-----|
| `000_session.sql` | 会话设置 |
| `010_DemoItems.sql` | DemoItems |
| `020_Vouchers.sql` | Vouchers |
| `030_ProcessedBlobRecords.sql` | ProcessedBlobRecords + 索引 |
| `040_ApplicationTypes.sql` | ApplicationTypes + 索引 |
| `050_ApplicationTypeFields.sql` | ApplicationTypeFields + 索引 |
| `060_CsvColumnMappings.sql` | CsvColumnMappings |
| `070_Departments.sql` | Departments + 索引 |
| `080_UserRoles.sql` | UserRoles + 索引（依赖 Departments） |

## 使用方式

### SSMS / Azure Data Studio

1. 连接到目标数据库。
2. 按上表顺序逐个打开并执行，或执行 `run-all.sql`（需在该目录下打开，以便 `:r` 相对路径正确）。

### sqlcmd（一键）

```bash
cd src/Infrastructure/Ddl
sqlcmd -S your-server.database.windows.net -d your-db -U user -P password -i run-all.sql
```

连接字符串参考 `src/Api/appsettings.Development.json` 的 `ConnectionStrings:DefaultConnection`。

## 增量变更

在本目录新增脚本，例如 `090_add_xxx_column.sql`，不要修改已发布环境的 `010`–`080` 初始脚本内容（除非团队约定可安全重复执行）。

## EF 实体对应

| 表 | 实体 |
|----|------|
| DemoItems | `Infrastructure.Entities.DemoItem` |
| Vouchers | `Infrastructure.Entities.Voucher` |
| ProcessedBlobRecords | `Infrastructure.Entities.ProcessedBlobRecord` |
| ApplicationTypes | `Infrastructure.Entities.ApplicationType` |
| ApplicationTypeFields | `Infrastructure.Entities.ApplicationTypeField` |
| CsvColumnMappings | `Infrastructure.Entities.CsvColumnMapping` |
| Departments | `Infrastructure.Entities.Department` |
| UserRoles | `Infrastructure.Entities.UserRole` |

## 种子数据

开发环境 API 首次启动时，`AuthDataSeeder` 会在无科室数据时插入默认 `DEV` 科室（及可选开发用户角色）。业务数据通过应用或单独 DML 脚本维护。
