FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
RUN apt-get update && apt-get install -y curl
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["BoxFactoryAPI/BoxFactoryAPI.csproj", "BoxFactoryAPI/"]
COPY ["Core/Core.csproj", "Core/"]
COPY ["Infrastructure/Infrastructure.csproj", "Infrastructure/"]
COPY ["Models/Models.csproj", "Models/"]
RUN dotnet restore "BoxFactoryAPI/BoxFactoryAPI.csproj"
COPY . .
WORKDIR "/src/BoxFactoryAPI"
RUN dotnet build "./BoxFactoryAPI.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./BoxFactoryAPI.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
HEALTHCHECK CMD curl --fail http://localhost:8080/health || exit 1
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "BoxFactoryAPI.dll"]
