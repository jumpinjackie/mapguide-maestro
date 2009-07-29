SET TYPEBUILD=Release
SET MAESTRO_DEV=..\Maestro
SET MAESTRO_OUTPUT=..\InstallerTemp%TYPEBUILD%\MapGuideMaestro
SET MAESTRO_WIX=..\Installer
SET PARAFFIN_PATH=%MAESTRO_WIX%\paraffin.exe
SET XCOPY=xcopy /E /Y /I /Q

%XCOPY% "..\Maestro\bin\%TYPEBUILD%" "%MAESTRO_OUTPUT%"
del /Q "%MAESTRO_OUTPUT%"\*.vshost.*
del /Q "%MAESTRO_OUTPUT%"\*.pdb
del /S /Q "%MAESTRO_OUTPUT%"\Localization\default > NUL
del /S /Q "%MAESTRO_OUTPUT%"\*.db > NUL
rmdir "%MAESTRO_OUTPUT%"\Localization\default

%PARAFFIN_PATH% -dir %MAESTRO_OUTPUT% -alias "%MAESTRO_OUTPUT%" -custom MAESTROBIN -dirref INSTALLLOCATION -multiple -guids incBinFiles.wxs -ext .pdb -direXclude .svn -direXclude Localization

rem del /Q "%MAESTRO_WIX%\incLang.wxs"

for /D %%d in ("%MAESTRO_OUTPUT%\Localization\*") do call :langbuild %%d

WixProjBuilder.exe Maestro.wixproj

goto end_of_program

:langbuild
set FOLDERNAME=
for /f "tokens=*" %%a in ("%1") do set FOLDERNAME=%%~na
%PARAFFIN_PATH% -dir %1 -alias %1 -custom MAESTROBIN_%FOLDERNAME% -dirref LANGLOCATION -multiple -guids incLocFiles.%FOLDERNAME%.wxs -ext .pdb -direXclude .svn -direXclude Localization
rem echo "<?include incLocFiles.%FOLDERNAME%.wxs ?>" >> "%MAESTRO_WIX%\incLang.wxs"
goto end_of_program


:end_of_program