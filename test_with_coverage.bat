@echo off
SET CONFIG=%1
SET TARGETFX=net461
IF NOT EXIST Reports MKDIR Reports
Packages\opencover\4.6.519\tools\OpenCover.Console.exe -filter:"+[OSGeo.*]* -[*.tests]* -[*.Tests]* -[Irony]*" -register:user -target:"Packages\xunit.runner.console\2.3.0\tools\net452\xunit.console.exe" -targetargs:"OSGeo.MapGuide.MaestroAPI.Tests\bin\%CONFIG%\%TARGETFX%\OSGeo.MapGuide.MaestroAPI.Tests.dll OSGeo.MapGuide.ObjectModels.Tests\bin\%CONFIG%\%TARGETFX%\OSGeo.MapGuide.ObjectModels.Tests.dll -noshadow" -output:Reports\OpenCoverCoverage.xml -coverbytest:*.Tests.dll
IF %ERRORLEVEL% NEQ 0 EXIT /B %ERRORLEVEL%
Packages\reportgenerator\3.0.0\tools\ReportGenerator.exe -reports:Reports\OpenCoverCoverage.xml -targetdir:Reports