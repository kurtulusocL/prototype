FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build-env
WORKDIR /app
COPY . ./
RUN dotnet publish -c Release -o out

# Çalıştırma için gerekli olan runtime imajı
FROM mcr.microsoft.com/dotnet/aspnet:9.0
WORKDIR /app
COPY --from=build-env /app/out .
ENTRYPOINT ["dotnet", "Prototype.WebUI.dll"]