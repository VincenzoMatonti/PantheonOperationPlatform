FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build

WORKDIR /src

COPY . .

RUN dotnet restore src/Hephaestus/Hephaestus.Worker/Hephaestus.Worker.csproj

RUN dotnet publish \
    src/Hephaestus/Hephaestus.Worker/Hephaestus.Worker.csproj \
    -c Release \
    -o /app/publish \
    --no-restore

FROM mcr.microsoft.com/dotnet/runtime:10.0 AS runtime

WORKDIR /app

COPY --from=build /app/publish .

ENV DOTNET_ENVIRONMENT=Development

ENTRYPOINT ["dotnet", "Hephaestus.Worker.dll"]