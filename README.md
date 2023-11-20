# dotnet-diagnostics-workshop

**Images bauen:**  
docker build -t dotnet_diagnostics_workshop -f .\code\DockerDotnetTest\DockerDotnetTest\Dockerfile .\code\DockerDotnetTest
docker build -t sqlserver_diagnostics -f .\SqlServer\Dockerfile .\SqlServer

**SQL Container ausführen:**
docker run --rm -p 1433:1433 --name sqlserver_diagnostics  sqlserver_diagnostics 

**Netzwerk erstellen und IP für SQL Container vergeben:**
docker network create diagnostics
docker network connect diagnostics sqlserver_diagnostics

**IP von SQL Container holen:**
docker network inspect diagnostics
Dort die IP vom SQL Container auslesen

**App Container starten und vernetzten:**
docker run --rm -p 5000:5000 --name dotnet_diagnostics_workshop -e SQLIP=<ip von sqlserver_diagnostics>  dotnet_diagnostics_workshop 
docker network connect diagnostics dotnet_diagnostics_workshop 