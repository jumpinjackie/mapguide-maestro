@echo off
set CONF=%1
if "%CONF%"=="" set CONF=Release
pushd Maestro
dotnet publish -r win-x64 -c %CONF% --self-contained true .\Maestro.csproj -o ..\out\publish
popd
REM copy addins
REM ExtendedObjectModels
if not exist "%CD%\out\publish\AddIns\ExtendedObjectModels" mkdir "%CD%\out\publish\AddIns\ExtendedObjectModels"
copy /Y "%CD%\Maestro.AddIn.ExtendedObjectModels\bin\%CONF%\Maestro.AddIn.ExtendedObjectModels.dll" "%CD%\out\publish\AddIns\ExtendedObjectModels"
copy /Y "%CD%\Maestro.AddIn.ExtendedObjectModels\bin\%CONF%\Manifest.addin" "%CD%\out\publish\AddIns\ExtendedObjectModels"
REM Rest
if not exist "%CD%\out\publish\AddIns\Rest" mkdir "%CD%\out\publish\AddIns\Rest"
copy /Y "%CD%\Maestro.AddIn.Rest\bin\%CONF%\Maestro.AddIn.Rest.dll" "%CD%\out\publish\AddIns\Rest"
copy /Y "%CD%\Maestro.AddIn.Rest\bin\%CONF%\Manifest.addin" "%CD%\out\publish\AddIns\Rest"
REM Scripting
if not exist "%CD%\out\publish\AddIns\Scripting" mkdir "%CD%\out\publish\AddIns\Scripting"
copy /Y "%CD%\Maestro.AddIn.Scripting\bin\%CONF%\Maestro.AddIn.Scripting.dll" "%CD%\out\publish\AddIns\Scripting"
copy /Y "%CD%\Maestro.AddIn.Scripting\bin\%CONF%\Manifest.addin" "%CD%\out\publish\AddIns\Scripting"