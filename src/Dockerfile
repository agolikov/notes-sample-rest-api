#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0-buster-slim AS build
WORKDIR /src
COPY ["notes.api/notes.api.csproj", ""]
COPY ["notes.application/notes.application.csproj", "./notes.application/"]
COPY ["notes.data/notes.data.csproj", "./notes.data/"]
RUN dotnet restore "./notes.api.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "notes.api.sln" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "notes.api.sln" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
CMD ASPNETCORE_URLS=http://*:$PORT dotnet notes.api.dll
#ENTRYPOINT ["dotnet", "notes.api.dll"]