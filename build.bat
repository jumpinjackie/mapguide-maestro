@echo off
SET TYPEACTION=build
SET TYPEBUILD=Release
SET PLATFORM=Any CPU
SET RELEASE_VERSION=5.0b1
SET OLDPATH=%PATH%
SET PATH=%PATH%;%CD%\Thirdparty\NSIS;C:\Windows\Microsoft.NET\Framework\v4.0.30319
SET SLNDIR=%CD%

:study_params
if (%1)==() goto start_build

if "%1"=="-help"    goto help_show
if "%1"=="-h"       goto help_show

if "%1"=="-c"       goto get_conf
if "%1"=="-config"  goto get_conf

if "%1"=="-a"       goto get_action
if "%1"=="-action"  goto get_action

if "%1"=="-p"        goto get_platform
if "%1"=="-platform" goto get_platform

if "%1"=="-version" goto get_version

if "%1"=="-v"       goto get_verbose
if "%1"=="-verbose" goto get_verbose

goto custom_error

:next_param
shift
shift
goto study_params

:get_verbose
SET VERBOSITY=/v:n
goto next_param

:get_version
SET RELEASE_VERSION=%2
goto next_param

:get_conf
SET TYPEBUILD=%2
SET OUTDIR=%CD%\out\%PLATFORM%\%TYPEBUILD%
if "%2"=="release" goto next_param
if "%2"=="Release" goto next_param
if "%2"=="debug" goto next_param
if "%2"=="Debug" goto next_param
goto custom_error

:get_action
SET TYPEACTION=%2
if "%2"=="build" goto next_param
if "%2"=="clean" goto next_param
goto custom_error

:get_platform
SET PLATFORM=%2
SET OUTDIR=%CD%\out\%PLATFORM%\%TYPEBUILD%
if "%2"=="x86" goto next_param
if "%2"=="x64" goto next_param
goto custom_error

:start_build
SET MSBUILD=msbuild.exe /p:Configuration=%TYPEBUILD%
SET MAKENSIS=makensis.exe /DSLN_CONFIG=%TYPEBUILD% /DCPU=x86 /DRELEASE_VERSION=%RELEASE_VERSION%

if "%TYPEACTION%"=="build" goto build
if "%TYPEACTION%"=="clean" goto clean

:build
pushd Maestro
%MSBUILD% Maestro_All.sln
popd
rem GeoRest addin not ready for prime-time so remove this from output dir
pushd out\%TYPEBUILD%\AddIns
rd /S /Q GeoRest
popd
pushd UserDoc
call make.bat html
popd
pushd UserDoc\build\html
xcopy /S /Y *.* %SLNDIR%\out\%TYPEBUILD%\UserDoc\
popd
pushd Install
%MAKENSIS% Maestro.nsi
popd
goto quit

:clean
pushd Maestro
%MSBUILD% /t:clean Maestro_All.sln
popd
rd /S /Q out
goto quit

:custom_error
echo The command is not recognized.
echo Please use the format:
:help_show
echo ************************************************************************
echo build.bat [-h]
echo           [-t]
echo           [-v]
echo           [-p=CPU]
echo           [-c=BuildType]
echo           [-a=Action]
echo Help:                  -h[elp]
echo Test:                  -t[est]
echo Verbose:               -v
echo CPU:                   -p[latform]=x86(default),x64
echo BuildType:             -c[onfig]=Release(default), Debug
echo Action:                -a[ction]=build(default),
echo                                  clean
echo ************************************************************************
:quit
SET PATH=%OLDPATH%