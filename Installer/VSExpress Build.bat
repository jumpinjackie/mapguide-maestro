SET TYPEBUILD=Release
SET MAESTRO_DEV=..\Maestro
SET MAESTRO_LOCALIZATION=%CD%\..\Localization
SET MAESTRO_OUTPUT=..\InstallerTemp%TYPEBUILD%\MapGuideMaestro
SET OUTPUT_ZIPFILE=bin\%TYPEBUILD%\MapGuide Maestro.zip
SET MAESTRO_WIX=%CD%
SET PARAFFIN_PATH=%MAESTRO_WIX%\paraffin.exe
SET XCOPY=xcopy /E /Y /I /Q
SET ZIP=%PROGRAMFILES%\7-zip\7z.exe
SET DELTREE=rmdir /S /Q

%DELTREE% "%MAESTRO_OUTPUT%"

%XCOPY% "..\Maestro\bin\%TYPEBUILD%" "%MAESTRO_OUTPUT%"
del /Q "%MAESTRO_OUTPUT%"\*.vshost.*
del /Q "%MAESTRO_OUTPUT%"\*.pdb
del /S /Q "%MAESTRO_OUTPUT%"\*.db > NUL

pushd %MAESTRO_LOCALIZATION%
%DELTREE% compiled
LocalizationTool.exe update
LocalizationTool.exe build
for /D %%d in ("compiled\*") do call :langbuild %%d
popd

"%PARAFFIN_PATH%" -dir %MAESTRO_OUTPUT% -alias "%MAESTRO_OUTPUT%" -custom MAESTROBIN -dirref INSTALLLOCATION -multiple -guids incBinFiles.wxs -ext .pdb -direXclude .svn

WixProjBuilder.exe Maestro.wixproj

rem Move in localization files and build zip
%XCOPY% "%MAESTRO_LOCALIZATION%\compiled\*" "%MAESTRO_OUTPUT%"
del /Q "%OUTPUT_ZIPFILE%"
"%ZIP%" a -r "%OUTPUT_ZIPFILE%" "%MAESTRO_OUTPUT%"

goto end_of_program

:langbuild
set FOLDERNAME=
for /f "tokens=*" %%a in ("%1") do set FOLDERNAME=%%~na
"%PARAFFIN_PATH%" -dir %1 -alias ..\Localization\%1 -custom MAESTROBIN_%FOLDERNAME% -dirref dir_MapGuideMaestro_0 -multiple -guids "%MAESTRO_WIX%\incLocFiles.%FOLDERNAME%.wxs" -ext .pdb -direXclude .svn
goto end_of_program


:end_of_program