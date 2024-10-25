FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["notes.api/notes.api.csproj", "notes.api/"]
COPY ["notes.application/notes.application.csproj", "notes.application/"]
COPY ["notes.data/notes.data.csproj", "notes.data/"]
RUN dotnet restore "notes.api/notes.api.csproj"
COPY . .
WORKDIR "/src/notes.api"
RUN dotnet build "notes.api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "notes.api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "notes.api.dll"]
