set ALL_SQL_FILE=AllSQL.sql
if exist %ALL_SQL_FILE% del %ALL_SQL_FILE%
forfiles /M *.sql /c "cmd /c type @file >> %ALL_SQL_FILE%"
set %ALL_SQL_FILE%=