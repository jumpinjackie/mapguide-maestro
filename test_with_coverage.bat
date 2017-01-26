@echo off
SET CONFIG=%1
IF NOT EXIST Reports MKDIR Reports
Maestro\packages\OpenCover.4.6.519\tools\OpenCover.Console.exe -filter:"+[OSGeo.*]* -[*.tests]* -[*.Tests]* -[Irony]*" -register:user -target:"Maestro\packages\NUnit.ConsoleRunner.3.6.0\tools\nunit3-console.exe" -targetargs:"OSGeo.MapGuide.MaestroAPI.Tests\bin\%CONFIG%\OSGeo.MapGuide.MaestroAPI.Tests.dll OSGeo.MapGuide.ObjectModel.Tests\bin\%CONFIG%\OSGeo.MapGuide.ObjectModels.Tests.dll" -output:Reports\OpenCoverCoverage.xml
IF %ERRORLEVEL% NEQ 0 EXIT /B %ERRORLEVEL%
Maestro\packages\ReportGenerator.2.5.2\tools\ReportGenerator.exe -reports:Reports\OpenCoverCoverage.xml -targetdir:Reports