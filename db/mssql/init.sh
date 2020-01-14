sleep 15s
/opt/mssql-tools/bin/sqlcmd -S localhost -U SA -P $SA_PASSWORD -d master -i prod.sql
/opt/mssql-tools/bin/sqlcmd -S localhost -U SA -P $SA_PASSWORD -d master -i test.sql