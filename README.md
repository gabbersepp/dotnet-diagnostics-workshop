# dotnet-diagnostics-workshop

**Images bauen:**  

```
docker build -t gabbersepp/dotnet_diagnostics_workshop -f .\code\DockerDotnetTest\DockerDotnetTest\Dockerfile .\code\DockerDotnetTest
docker build -t gabbersepp/sqlserver_diagnostics -f .\SqlServer\Dockerfile .\SqlServer
```

**Images pushen**

```
docker push gabbersepp/dotnet_diagnostics_workshop
docker push gabbersepp/sqlserver_diagnostics
```

**SQL Container ausführen:**

```
docker run --rm -p 1433:1433 --name sqlserver_diagnostics  gabbersepp/sqlserver_diagnostics 
```

**Netzwerk erstellen und IP für SQL Container vergeben:**

```
docker network create diagnostics
docker network connect diagnostics sqlserver_diagnostics
```

**IP von SQL Container holen:**

```
docker network inspect diagnostics
```
Dort die IP vom SQL Container auslesen

**App Container starten und vernetzten:**

```
docker run --rm -p 5000:5000 --name dotnet_diagnostics_workshop -e SQLIP=<ip con sql container>  gabbersepp/dotnet_diagnostics_workshop 
docker network connect diagnostics dotnet_diagnostics_workshop 
```