#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS base
RUN dotnet tool install -g dotnet-dump
RUN dotnet tool install -g dotnet-trace
RUN dotnet tool install -g dotnet-stack
RUN dotnet tool install -g dotnet-symbol
RUN dotnet tool install -g dotnet-gcdump
RUN dotnet tool install -g dotnet-counters
RUN apt-get update
RUN dotnet tool install -g dotnet-sos
RUN /root/.dotnet/tools/dotnet-sos install
RUN apt-get install lldb -y
RUN apt-get update && apt-get install -y procps && rm -rf /var/lib/apt/lists/*
ENV PATH="/root/.dotnet/tools/:${PATH}"
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["DockerDotnetTest/DockerDotnetTest.csproj", "DockerDotnetTest/"]
RUN dotnet restore "DockerDotnetTest/DockerDotnetTest.csproj"
COPY . .
WORKDIR "/src/DockerDotnetTest"
RUN dotnet build "DockerDotnetTest.csproj" -c Debug -o /app/build

FROM build AS publish
RUN dotnet publish "DockerDotnetTest.csproj" -c Debug -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "DockerDotnetTest.dll"]