dotnet test /p:CollectCoverage=true /p:CoverletOutputFormat=cobertura Core.sln
if not exist codecov mkdir codecov
reportgenerator.exe -reports:%CD%\OSGeo.MapGuide.MaestroAPI.Tests\coverage.net6.0.cobertura.xml -targetdir:%CD%\codecov