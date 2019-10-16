if "%APPVEYOR_BUILD_FOLDER%" == "" SET APPVEYOR_BUILD_FOLDER=%CD%
if "%CONFIGURATION%" == "" SET CONFIGURATION=Release
if "%NETCORE_MONIKER%" == "" SET NETCORE_MONIKER=netcoreapp2.2
if "%APPVEYOR_BUILD_NUMBER%" == "" SET APPVEYOR_BUILD_NUMBER=0
if "%APPVEYOR_REPO_TAG%" == "true" set ARTIFACT_RELEASE_LABEL=%APPVEYOR_REPO_TAG_NAME%
if "%ARTIFACT_RELEASE_LABEL%" == "" SET ARTIFACT_RELEASE_LABEL=master

if not exist %APPVEYOR_BUILD_FOLDER%\artifacts mkdir %APPVEYOR_BUILD_FOLDER%\artifacts
cd /D %APPVEYOR_BUILD_FOLDER%\Docs
docfx build
cd /D %APPVEYOR_BUILD_FOLDER%\out\Release
7z a -mx9 %APPVEYOR_BUILD_FOLDER%\artifacts\MapGuideMaestro-%CONFIGURATION%-%ARTIFACT_RELEASE_LABEL%.zip * -x!LocalConfigure.* -x!Addins\Local -x!Addins\Scripting
cd /D %APPVEYOR_BUILD_FOLDER%\MgTileSeeder
dotnet publish -c %CONFIGURATION% -f %NETCORE_MONIKER% -r win-x64 -o %APPVEYOR_BUILD_FOLDER%\out\Release\tools\MgTileSeeder
dotnet publish -c %CONFIGURATION% -f %NETCORE_MONIKER% -r linux-x64 -o publish_linux
cd /D %APPVEYOR_BUILD_FOLDER%\Maestro.MapPublisher
dotnet publish -c %CONFIGURATION% -f %NETCORE_MONIKER% -r win-x64 -o %APPVEYOR_BUILD_FOLDER%\out\Release\tools\Maestro.MapPublisher
dotnet publish -c %CONFIGURATION% -f %NETCORE_MONIKER% -r linux-x64 -o publish_linux
cd /D %APPVEYOR_BUILD_FOLDER%\out\%CONFIGURATION%\Tools\MgTileSeeder
7z a -mx9 %APPVEYOR_BUILD_FOLDER%\artifacts\MgTileSeeder-win-x64-%ARTIFACT_RELEASE_LABEL%.zip *
cd /D %APPVEYOR_BUILD_FOLDER%\MgTileSeeder\publish_linux
7z a -mx9 %APPVEYOR_BUILD_FOLDER%\artifacts\MgTileSeeder-linux-x64-%ARTIFACT_RELEASE_LABEL%.zip *
cd /D %APPVEYOR_BUILD_FOLDER%\out\%CONFIGURATION%\Tools\Maestro.MapPublisher
7z a -mx9 %APPVEYOR_BUILD_FOLDER%\artifacts\Maestro-MapPublisher-win-x64-%ARTIFACT_RELEASE_LABEL%.zip *
cd /D %APPVEYOR_BUILD_FOLDER%\Maestro.MapPublisher\publish_linux
7z a -mx9 %APPVEYOR_BUILD_FOLDER%\artifacts\Maestro-MapPublisher-linux-x64-%ARTIFACT_RELEASE_LABEL%.zip *
cd /D %APPVEYOR_BUILD_FOLDER%\Install
appveyor DownloadFile https://download.microsoft.com/download/1/6/B/16B06F60-3B20-4FF2-B699-5E9B7962F9AE/VSU_4/vcredist_x86.exe
%APPVEYOR_BUILD_FOLDER%\Packages\nsis\2.51.0\tools\makensis.exe /DSLN_CONFIG=%CONFIGURATION% /DCPU=x86 /DRELEASE_VERSION=%ARTIFACT_RELEASE_LABEL% Maestro.nsi
cd %APPVEYOR_BUILD_FOLDER%
dotnet pack Core.sln --configuration %CONFIGURATION% --output %APPVEYOR_BUILD_FOLDER%\artifacts --include-symbols /p:Version=6.0.0-pre%APPVEYOR_BUILD_NUMBER%"