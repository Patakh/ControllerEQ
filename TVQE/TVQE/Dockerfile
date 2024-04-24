# Стадия сборки
FROM docker pull mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /app

# Копируем и восстанавливаем зависимости
COPY *.csproj ./
RUN dotnet restore

# Копируем и собираем приложение
COPY . ./
RUN dotnet build -c Release -o build

# Публикуем приложение для запуска
FROM docker pull mcr.microsoft.com/dotnet/runtime:6.0 AS runtime
WORKDIR /app
COPY --from=build /app/build ./

# Запускаем приложение при старте контейнера
ENTRYPOINT ["dotnet", "TVQE.dll"]