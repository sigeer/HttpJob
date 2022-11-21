#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["NuGet.Config", "."]
COPY ["MineServer/MineServer.csproj", "MineServer/"]
COPY ["SpiderRemoteServiceClient/SpiderRemoteServiceClient.csproj", "SpiderRemoteServiceClient/"]
COPY ["SpiderTool.Data/SpiderTool.Data.csproj", "SpiderTool.Data/"]
RUN dotnet restore "MineServer/MineServer.csproj"
COPY . .
WORKDIR "/src/MineServer"
RUN dotnet build "MineServer.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "MineServer.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MineServer.dll"]