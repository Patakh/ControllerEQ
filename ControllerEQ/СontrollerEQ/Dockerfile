# Стадия сборки
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /app

# Восстановление зависимостей
COPY *.csproj .
RUN dotnet restore

# Копирование и сборка приложения
COPY . .
RUN dotnet publish -c Release -o out

# Стадия развертывания
FROM mcr.microsoft.com/dotnet/runtime:6.0 AS runtime
WORKDIR /app
COPY --from=build /app/out .
ENTRYPOINT ["dotnet", "ControllerEQ.dll"]