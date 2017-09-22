@echo off
dotnet test OSGeo.MapGuide.MaestroAPI.Tests
IF %ERRORLEVEL% NEQ 0 exit /B %ERRORLEVEL%
dotnet test OSGeo.MapGuide.ObjectModels.Tests
IF %ERRORLEVEL% NEQ 0 exit /B %ERRORLEVEL%