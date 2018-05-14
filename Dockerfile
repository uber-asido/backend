FROM microsoft/dotnet:2.1.0-rc1-aspnetcore-runtime AS base
WORKDIR /app

FROM microsoft/dotnet:2.1.300-rc1-sdk AS build
WORKDIR /src
COPY . .
RUN dotnet restore
WORKDIR /src/src/Uber.Server.Gateway
RUN dotnet build --no-restore -c Release -o /app

FROM build AS publish
RUN dotnet publish --no-restore -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Uber.Server.Gateway.dll"]
