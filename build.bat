@echo off
rem MapGuide Maestro build script for windows
rem
rem Author: Jackie Ng (jumpinjackie@gmail.com)
rem
rem This script will build all the MapGuide Maestro components, if the output path is specified
rem it will also copy all binaries to the output path. Otherwise it will use the default
rem value of MAESTRO_OUTPUT defined in this file
rem 
rem Usage:
rem
rem build.bat [-h]
rem           [-v]
rem           [-c=BuildType]
rem           [-a=Action]
rem           [-o=OutputDirectory]
rem
rem BuildType: Release(default), Debug
rem Action: build(defualt), install, buildinstall, clean, prepare
rem OutputDirectory: The directory where files will be copied to if -a=install
rem
rem Please note that -a=install does nothing if -w=oem

rem ==================================================
rem Top-level vars
rem ==================================================
SET OLDPATH=%PATH%
SET TYPEACTION=build
SET TYPEBUILD=Release

rem ==================================================
rem MapGuide Maestro vars
rem ==================================================
SET MAESTRO_DEV=%CD%\Maestro
SET MAESTRO_OUTPUT=%CD%\InstallerTemp%TYPEBUILD%\MapGuideMaestro
SET MAESTRO_WIX=%CD%\Installer
SET PARAFFIN_PATH=%MAESTRO_WIX%\paraffin.exe

rem ==================================================
rem MSBuild Settings
rem ==================================================

rem If the NUMBER_OF_PROCESSORS environment variable is wrong for any reason. Change this value.
SET CPU_CORES=%NUMBER_OF_PROCESSORS%

rem Uncomment the line below to enable msbuild logging
rem SET MSBUILD_LOG=/l:FileLogger,Microsoft.Build.Engine;logfile=Build.log;verbosity=diagnostic
SET MSBUILD_VERBOSITY=/v:q

rem ==================================================
rem Command aliases
rem ==================================================
rem SET XCOPY=xcopy /E /Y /I /F
SET XCOPY=xcopy /E /Y /I /Q
SET MSBUILD=msbuild.exe /nologo /p:Configuration=%TYPEBUILD% %MSBUILD_VERBOSITY% %MSBUILD_LOG%
SET MSBUILD_CLEAN=msbuild.exe /nologo /m:%CPU_CORES% /p:Configuration=%TYPEBUILD% /t:Clean %MSBUILD_VERBOSITY%

:study_params
if (%1)==() goto start_build

if "%1"=="-help"    goto help_show
if "%1"=="-h"       goto help_show

if "%1"=="-c"       goto get_conf
if "%1"=="-config"  goto get_conf

if "%1"=="-a"       goto get_action
if "%1"=="-action"  goto get_action

if "%1"=="-v"       goto get_verbose
if "%1"=="-o"       goto get_output

goto custom_error

:next_param
shift
shift
goto study_params

:get_verbose
SET MSBUILD_VERBOSITY=/v:d
SET MSBUILD=msbuild.exe /nologo /m:%CPU_CORES% /p:Configuration=%TYPEBUILD% %MSBUILD_VERBOSITY% %MSBUILD_LOG%
SET MSBUILD_CLEAN=msbuild.exe /nologo /m:%CPU_CORES% /p:Configuration=%TYPEBUILD% /t:Clean %MSBUILD_VERBOSITY%
goto next_param

:get_output
SET MAESTRO_OUTPUT=%2
SET MAESTRO_OUTPUT_SERVER=%MAESTRO_OUTPUT%\Server
SET MAESTRO_OUTPUT_WEB=%MAESTRO_OUTPUT%\WebServerExtensions
goto next_param

:get_conf
SET TYPEBUILD=%2
SET MAESTRO_OUTPUT=%CD%\%TYPEBUILD%
SET MSBUILD=msbuild.exe /nologo /m:%CPU_CORES% /p:Configuration=%TYPEBUILD% %MSBUILD_VERBOSITY% %MSBUILD_LOG%
SET MSBUILD_CLEAN=msbuild.exe /nologo /m:%CPU_CORES% /p:Configuration=%TYPEBUILD% /t:Clean %MSBUILD_VERBOSITY%

if "%2"=="Release" goto next_param
if "%2"=="Debug" goto next_param
SET ERRORMSG=Unrecognised configuration: %2
goto custom_error

:get_action
SET TYPEACTION=%2
if "%2"=="build" goto next_param
if "%2"=="clean" goto next_param
if "%2"=="install" goto next_param
if "%2"=="update" goto next_param
if "%2"=="buildinstall" goto next_param
if "%2"=="regen" goto next_param
SET ERRORMSG=Unrecognised action: %2
goto custom_error

:start_build
echo ===================================================
echo Configuration is [%TYPEBUILD%]
echo Action is [%TYPEACTION%]
echo Deployment Directory for Maestro: %MAESTRO_OUTPUT%
echo CPU cores: %CPU_CORES%
echo ===================================================

if "%TYPEACTION%"=="build" goto build
if "%TYPEACTION%"=="clean" goto clean
if "%TYPEACTION%"=="install" goto install
if "%TYPEACTION%"=="update" goto update
if "%TYPEACTION%"=="buildinstall" goto build
if "%TYPEACTION%"=="regen" goto build

:clean
echo [Clean] MapGuide Maestro
pushd %MAESTRO_DEV%
%MSBUILD_CLEAN% OSGeo.MapGuide.Maestro.sln
popd
goto quit

:update
echo [update] MapGuide Maestro
pushd %MAESTRO_DEV%
call "Update %TYPEBUILD% Environment.bat"
popd
if not "%TYPEACTION%"=="buildinstall" goto quit

:build
echo [build] MapGuide Maestro
pushd %MAESTRO_DEV%
%MSBUILD% OSGeo.MapGuide.Maestro.sln
popd
if "%errorlevel%"=="1" goto error
if not "%TYPEACTION%"=="buildinstall" goto quit

:install
echo [install] MapGuide Maestro
%XCOPY% "%MAESTRO_DEV%\bin\%TYPEBUILD%" "%MAESTRO_OUTPUT%"
del /Q "%MAESTRO_OUTPUT%"\*.vshost.*
del /Q "%MAESTRO_OUTPUT%"\*.pdb
del /Q "%MAESTRO_OUTPUT%"\Localization\default

pushd %MAESTRO_WIX%
%PARAFFIN_PATH% -dir %MAESTRO_OUTPUT% -alias "%MAESTRO_OUTPUT%" -custom MAESTROBIN -dirref INSTALLLOCATION -multiple -guids incBinFiles.wxs -ext .pdb -direXclude .svn -direXclude Localization

rem del /Q "%MAESTRO_WIX%\incLang.wxs"

for /D %%d in ("%MAESTRO_OUTPUT%\Localization\*") do call :langbuild %%d

%MSBUILD% Installer.sln
popd
if "%errorlevel%"=="1" goto error
goto quit

:langbuild
set FOLDERNAME=
for /f "tokens=*" %%a in ("%1") do set FOLDERNAME=%%~na
%PARAFFIN_PATH% -dir %1 -alias %1 -custom MAESTROBIN_%FOLDERNAME% -dirref LANGLOCATION -multiple -guids incLocFiles.%FOLDERNAME%.wxs -ext .pdb -direXclude .svn -direXclude Localization
rem echo "<?include incLocFiles.%FOLDERNAME%.wxs ?>" >> "%MAESTRO_WIX%\incLang.wxs"
goto end_of_program


:error
echo [ERROR]: There was an error building the component
exit /B 1

:custom_error_no_help
echo [ERROR]: %ERRORMSG%
SET ERRORMSG=
exit /B 1

:custom_error
echo [ERROR]: %ERRORMSG%
SET ERRORMSG=
echo Please use the format:
:help_show
echo ************************************************************************
echo build.bat [-h]
echo           [-v]
echo           [-c=BuildType]
echo           [-a=Action]
echo           [-o=OutputDirectory]
echo Help:                  -h[elp]
echo Verbose:               -v
echo BuildType:             -c[onfig]=Release(default), Debug
echo Action:                -a[ction]=build(default),
echo                                  install,
echo                                  buildinstall,
echo                                  clean,
echo                                  update
echo ************************************************************************
:quit
SET TYPEACTION=
SET TYPEBUILD=
SET MAESTRO_OUTPUT=
SET MAESTRO_DEV=
SET MAESTRO_WIX=
SET MSBUILD_LOG=
SET MSBUILD_VERBOSITY=
SET XCOPY=
SET MSBUILD=
SET PARAFFIN_PATH=
SET PATH=%OLDPATH%
:end_of_program