set /p Server=Enter Server: 
set /p Username=Enter Username: 
set /p Password=Enter Password: 

if exist OutputLog.txt (
	del OutputLog.txt
)

sqlcmd -S %Server% -U %Username% -P %Password% -i Create_DB_and_Tables.sql >> OutputLog.txt

pause
