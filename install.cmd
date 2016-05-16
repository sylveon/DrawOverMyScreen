@echo off
>nul 2>&1 "%SYSTEMROOT%\system32\cacls.exe" "%SYSTEMROOT%\system32\config\system"
if '%errorlevel%' NEQ '0' (goto UACPrompt) else (goto gotAdmin)
:UACPrompt
echo Set UAC = CreateObject^("Shell.Application"^) > "%temp%\getadmin.vbs"
echo UAC.ShellExecute "%~s0", "", "", "runas", 1 >> "%temp%\getadmin.vbs"
"%temp%\getadmin.vbs"
goto End
:GotAdmin
if exist "%temp%\getadmin.vbs" (del "%temp%\getadmin.vbs")
pushd "%CD%"
CD /D "%~dp0"
:FileExists
cls
set RequiredFiles=1
IF NOT EXIST ".\DrawOverMyScreen.scr" (set RequiredFiles=0)
IF NOT EXIST ".\DrawOverMyScreenClient.exe" (set RequiredFiles=0)
IF /I %RequiredFiles% EQU 0 (
    echo One or more required files does not exists. Are you sure you successfully compiled? After making sure, please continue.
    pause
    goto FileExists
)
copy ".\DrawOverMyScreen.scr" "%SystemRoot%\DrawOverMyScreen.scr"
:CopySuccessCheck
IF NOT EXIST "%SystemRoot%\DrawOverMyScreen.scr" (
    echo Something bad happened while copying the file "DrawOverMyScreen.scr". Please try yourself copying it to "%SystemRoot%\DrawOverMyScreen.scr" then continue when it is done.
    pause
    goto CopySuccessCheck
)
:WhereIsClient
cls
echo Tip! To directly get the path of a file:
echo     1. Right-click on it while pressing "Shift".
echo     2. Click on "Copy as Path".
echo     3. Right-click in this command prompt.
echo     4. If the path has not pasted itself already, click on "Paste".
echo     5. Remove the quotes at the beginning and the end.
echo     6. Press "Enter" to validate.
echo.
echo.
set /P Client=Move the client at any desired permanent location, and then type it's location here without quotes:
IF NOT DEFINED Client (
    cls
    echo You did not specify a file!
    pause
    goto WhereIsClient
)
IF NOT EXIST "%Client%" (
    cls
    echo The file you specified does not exists!
    pause
    goto WhereIsClient
)
cls
"%Client%" /register
rundll32.exe shell32.dll,Control_RunDLL desk.cpl,,1
:End