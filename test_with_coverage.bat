@echo off
SET CONFIG=%1
IF NOT EXIST Reports MKDIR Reports
Packages\opencover\4.6.519\tools\OpenCover.Console.exe -oldstyle -filter:"+[OSGeo.*]* -[*.tests]* -[*.Tests]* -[Irony]*" -register:user -target:"unittest.bat" -output:Reports\OpenCoverCoverage.xml
IF %ERRORLEVEL% NEQ 0 EXIT /B %ERRORLEVEL%
Packages\reportgenerator\3.0.0\tools\ReportGenerator.exe -reports:Reports\OpenCoverCoverage.xml -targetdir:Reports