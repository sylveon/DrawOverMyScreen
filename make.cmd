@echo off
rem If anyone has time to add fail checks, feel free to contribute.
IF NOT EXIST ".\nuget.exe" (
    echo Downloading NuGet...
    powershell -command "& { iwr https://dist.nuget.org/win-x86-commandline/v3.3.0/nuget.exe -OutFile nuget.exe }
    echo.
    echo.
    echo.
)
IF NOT EXIST ".\Microsoft.Net.Compilers.1.2.1\tools\csc.exe" (
    echo Installing compiler...
    .\nuget.exe install Microsoft.Net.Compilers -Version 1.2.1
    echo.
    echo.
    echo.
)
IF NOT EXIST ".\ILRepack.2.0.10\tools\ILRepack.exe" (
    echo Installing assembly merger...
    nuget install ILRepack -Version 2.0.10
    echo.
    echo.
    echo.
)
IF NOT EXIST ".\TaskScheduler.2.5.18\lib\net40\Microsoft.Win32.TaskScheduler.dll" (
    echo Installing dependencies...
    nuget install TaskScheduler -Version 2.5.18
    echo.
    echo.
    echo.
)
echo Compiling...
.\Microsoft.Net.Compilers.1.2.1\tools\csc.exe DrawOverMyScreen.cs /target:winexe /out:DrawOverMyScreen.scr /debug
.\Microsoft.Net.Compilers.1.2.1\tools\csc.exe DrawOverMyScreenClient.cs /target:winexe /r:TaskScheduler.2.5.18\lib\net40\Microsoft.Win32.TaskScheduler.dll /debug
echo.
echo.
echo.
echo Merging assemblies...
.\ILRepack.2.0.10\tools\ILRepack.exe /out:DrawOverMyScreenClient.exe .\DrawOverMyScreenClient.exe .\TaskScheduler.2.5.18\lib\net40\Microsoft.Win32.TaskScheduler.dll
echo.
echo.
echo.
echo Done!