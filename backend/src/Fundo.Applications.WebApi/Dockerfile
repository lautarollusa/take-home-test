FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 5000
EXPOSE 5001

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY backend/src .
RUN dotnet restore Fundo.Applications.WebApi/Fundo.Applications.WebApi.csproj
RUN dotnet publish Fundo.Applications.WebApi/Fundo.Applications.WebApi.csproj -c Release -o /app/publish

FROM base
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "Fundo.Applications.WebApi.dll"]