#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["NuGet.Config", "."]
COPY ["SpiderService/SpiderService.csproj", "SpiderService/"]
COPY ["SpiderTool.SqlSuger/SpiderTool.SqlSugar.csproj", "SpiderTool.SqlSuger/"]
COPY ["SpiderTool/SpiderTool.csproj", "SpiderTool/"]
COPY ["SpiderTool.Data/SpiderTool.Data.csproj", "SpiderTool.Data/"]
RUN dotnet restore "SpiderService/SpiderService.csproj"
COPY . .
WORKDIR "/src/SpiderService"
RUN dotnet build "SpiderService.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "SpiderService.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SpiderService.dll"]