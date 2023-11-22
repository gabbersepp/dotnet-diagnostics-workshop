Hallo, etwas kurzfristig, aber lieber später als nie.

bitte führe folgende Schritte aus:

docker pull gabbersepp/sqlserver_diagnostics 
docker pull gabbersepp/dotnet_diagnostics_workshop

docker run --rm -p 1433:1433 --name sqlserver_diagnostics  gabbersepp/sqlserver_diagnostics 
docker network create diagnostics
docker network connect diagnostics sqlserver_diagnostics


Dann rufe diesen Befehl auf, um dir die IP des SQL Containers anzeigen zu lassen:

docker network inspect diagnostics

Vom Container "sqlserver_diagnostics" bitte die IPv4 Adresse holen. In meinem Fall z.B. 172.19.0.3

Dann:

docker run --rm -p 5000:5000 --name dotnet_diagnostics_workshop -e SQLIP=<ip con sql container>  gabbersepp/dotnet_diagnostics_workshop 
docker network connect diagnostics dotnet_diagnostics_workshop 


Wenn etwas nicht geht, bitte direkt an gabbersepp@googlemail.com schreiben, sodass wir Probleme möglichst noch vor dem Workshopstart klären können