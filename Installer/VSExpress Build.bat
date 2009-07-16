SET TYPEBUILD=Release
SET MAESTRO_DEV=..\Maestro
SET MAESTRO_OUTPUT=..\InstallerTemp%TYPEBUILD%\MapGuideMaestro
SET MAESTRO_WIX=..\Installer
SET PARAFFIN_PATH=%MAESTRO_WIX%\paraffin.exe
SET XCOPY=xcopy /E /Y /I /Q

%XCOPY% "..\Maestro\bin\%TYPEBUILD%" "%MAESTRO_OUTPUT%"
del /Q "%MAESTRO_OUTPUT%"\*.vshost.*
del /Q "%MAESTRO_OUTPUT%"\*.pdb

%PARAFFIN_PATH% -dir ..\InstallerTemp%TYPEBUILD%\MapGuideMaestro -alias ..\InstallerTemp%TYPEBUILD%\MapGuideMaestro -custom MAESTROBIN -dirref INSTALLLOCATION -multiple -guids incBinFiles.wxs -ext .pdb -direXclude .svn

WixProjBuilder.exe Maestro.wixproj

