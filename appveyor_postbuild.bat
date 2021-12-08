if "%APPVEYOR_BUILD_FOLDER%" == "" SET APPVEYOR_BUILD_FOLDER=%CD%
if "%CONFIGURATION%" == "" SET CONFIGURATION=Release
if "%APPVEYOR_BUILD_NUMBER%" == "" SET APPVEYOR_BUILD_NUMBER=0
if "%APPVEYOR_REPO_TAG%" == "true" set ARTIFACT_RELEASE_LABEL=%APPVEYOR_REPO_TAG_NAME%
if "%ARTIFACT_RELEASE_LABEL%" == "" SET ARTIFACT_RELEASE_LABEL=master

if not exist %APPVEYOR_BUILD_FOLDER%\artifacts mkdir %APPVEYOR_BUILD_FOLDER%\artifacts
cd /D %APPVEYOR_BUILD_FOLDER%\Docs
docfx build
IF %ERRORLEVEL% NEQ 0 exit /B %ERRORLEVEL%
cd /D %APPVEYOR_BUILD_FOLDER%\out\publish\Release
7z a -mx9 %APPVEYOR_BUILD_FOLDER%\artifacts\MapGuideMaestro-%CONFIGURATION%-%ARTIFACT_RELEASE_LABEL%.zip * -x!LocalConfigure.* -x!Addins\Local
IF %ERRORLEVEL% NEQ 0 exit /B %ERRORLEVEL%
REM TODO: Activate single-file/trimmed/compressed flags once we have verified this combo works without adverse side-effects
cd /D %APPVEYOR_BUILD_FOLDER%\MgTileSeeder
dotnet publish -c %CONFIGURATION% -r win-x64 -o %APPVEYOR_BUILD_FOLDER%\out\%CONFIGURATION%\tools\MgTileSeeder
IF %ERRORLEVEL% NEQ 0 exit /B %ERRORLEVEL%
dotnet publish -c %CONFIGURATION% -r linux-x64 -o publish_linux
IF %ERRORLEVEL% NEQ 0 exit /B %ERRORLEVEL%
cd /D %APPVEYOR_BUILD_FOLDER%\Maestro.MapPublisher
dotnet publish -c %CONFIGURATION% -r win-x64 -o %APPVEYOR_BUILD_FOLDER%\out\%CONFIGURATION%\tools\Maestro.MapPublisher
IF %ERRORLEVEL% NEQ 0 exit /B %ERRORLEVEL%
dotnet publish -c %CONFIGURATION% -r linux-x64 -o publish_linux
IF %ERRORLEVEL% NEQ 0 exit /B %ERRORLEVEL%
cd /D %APPVEYOR_BUILD_FOLDER%\out\%CONFIGURATION%\Tools\MgTileSeeder
7z a -mx9 %APPVEYOR_BUILD_FOLDER%\artifacts\MgTileSeeder-win-x64-%ARTIFACT_RELEASE_LABEL%.zip *
IF %ERRORLEVEL% NEQ 0 exit /B %ERRORLEVEL%
cd /D %APPVEYOR_BUILD_FOLDER%\MgTileSeeder\publish_linux
7z a -mx9 %APPVEYOR_BUILD_FOLDER%\artifacts\MgTileSeeder-linux-x64-%ARTIFACT_RELEASE_LABEL%.zip *
IF %ERRORLEVEL% NEQ 0 exit /B %ERRORLEVEL%
cd /D %APPVEYOR_BUILD_FOLDER%\out\%CONFIGURATION%\Tools\Maestro.MapPublisher
7z a -mx9 %APPVEYOR_BUILD_FOLDER%\artifacts\Maestro-MapPublisher-win-x64-%ARTIFACT_RELEASE_LABEL%.zip *
IF %ERRORLEVEL% NEQ 0 exit /B %ERRORLEVEL%
cd /D %APPVEYOR_BUILD_FOLDER%\Maestro.MapPublisher\publish_linux
7z a -mx9 %APPVEYOR_BUILD_FOLDER%\artifacts\Maestro-MapPublisher-linux-x64-%ARTIFACT_RELEASE_LABEL%.zip *
IF %ERRORLEVEL% NEQ 0 exit /B %ERRORLEVEL%
cd /D %APPVEYOR_BUILD_FOLDER%\Install
appveyor DownloadFile https://aka.ms/vs/17/release/vc_redist.x64.exe
%APPVEYOR_BUILD_FOLDER%\Packages\nsis\2.51.0\tools\makensis.exe /DSLN_CONFIG=%CONFIGURATION% /DCPU=x64 /DRELEASE_VERSION=%ARTIFACT_RELEASE_LABEL% Maestro.nsi
IF %ERRORLEVEL% NEQ 0 exit /B %ERRORLEVEL%
cd %APPVEYOR_BUILD_FOLDER%
dotnet pack Core.sln --configuration %CONFIGURATION% --output %APPVEYOR_BUILD_FOLDER%\artifacts /p:Version=6.0.0-pre%APPVEYOR_BUILD_NUMBER%"
IF %ERRORLEVEL% NEQ 0 exit /B %ERRORLEVEL%