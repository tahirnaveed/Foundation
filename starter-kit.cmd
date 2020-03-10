@echo off
cd /d %~dp0
openfiles > NUL 2>&1
if %ERRORLEVEL% NEQ 0 (
	set "errorMessage=Build.cmd script must be run in an elevated (admin) command prompt"
	goto error
)

mode con:cols=120 lines=5000
set ROOTPATH=%cd%
set ROOTDIR=%cd%
set SOURCEPATH=%ROOTPATH%\src
set APPCMD=%windir%\system32\inetsrv\appcmd.exe

:loop
set APPNAME=
set FOUNDATIONDOMAIN=
set CMDOMAIN=
set LICENSEPATH=
set SQLSERVER=
set ADDITIONAL_SQLCMD=
set /p APPNAME=Enter your application name (required):
set /p FOUNDATIONDOMAIN=Enter your public domain name for foundation(optional, press Enter to leave it blank):
set /p CMDOMAIN=Enter your public domain name for commerce manager(optional, press Enter to leave it blank):
set /p LICENSEPATH=Enter your LICENSE path (optional, press Enter to leave it blank):
set /p SQLSERVER=Enter your SQL server name (optional, press Enter for default (.) local server):
set /p ADDITIONAL_SQLCMD=Enter your sqlcmd command (optional, press Enter for default (-E) windows auth):

set check=false
if "%APPNAME%"=="" (set check=true)
if "%check%"=="true" (
	echo Parameters missing, application name is required, foundation domain name, commerce manager domain name and LICENSE path are optional
	pause
	cls
	goto loop
)

:main
if "%FOUNDATIONDOMAIN%"=="" (set FOUNDATIONDOMAIN="%APPNAME%")
if "%CMDOMAIN%"=="" (set CMDOMAIN="%APPNAME%-cm")
if "%SQLSERVER%"=="" (set SQLSERVER=.)
if "%ADDITIONAL_SQLCMD%"=="" (set ADDITIONAL_SQLCMD=-E)
if "%LICENSEPATH%"=="" (set LICENSEPATH="")

cls
echo Your application name is: %APPNAME%
echo Your foundation domain name is: %FOUNDATIONDOMAIN%
echo Your commerce manager domain name is: %CMDOMAIN%
echo Your LICENSE path is: %LICENSEPATH%
echo Your SQL server name is: %SQLSERVER%
echo Your SQLCMD command is: sqlcmd -S %SQLSERVER% %ADDITIONAL_SQLCMD%
timeout 15