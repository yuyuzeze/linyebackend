/*
    Run all initial DDL scripts in order (sqlcmd).
    From repo root:
      sqlcmd -S <server> -d <database> -U <user> -P <password> -i src/Infrastructure/Ddl/run-all.sql

    Or cd to this folder first so :r paths resolve:
      cd src/Infrastructure/Ddl
      sqlcmd ... -i run-all.sql
*/
:r 000_session.sql
:r 010_DemoItems.sql
:r 020_Vouchers.sql
:r 030_ProcessedBlobRecords.sql
:r 040_ApplicationTypes.sql
:r 050_ApplicationTypeFields.sql
:r 060_CsvColumnMappings.sql
:r 070_Departments.sql
:r 080_UserRoles.sql
