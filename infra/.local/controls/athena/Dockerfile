FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build

WORKDIR /src

COPY . .

RUN dotnet restore src/Athena/Athena.Runtime/Athena.Runtime.csproj

RUN dotnet publish src/Athena/Athena.Runtime/Athena.Runtime.csproj \
    -c Release \
    -o /app/publish \
    --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime

RUN apt-get update \
    && apt-get install -y --no-install-recommends docker.io \
    && rm -rf /var/lib/apt/lists/*


WORKDIR /app

COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "Athena.Runtime.dll"]
