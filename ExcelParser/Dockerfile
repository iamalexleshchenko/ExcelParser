# Этап сборки (build stage)
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
EXPOSE 5190
EXPOSE 5191

# Копируем только файл проекта и восстанавливаем зависимости
COPY ["ExcelParser.csproj", "."]
RUN dotnet restore "ExcelParser.csproj"

# Копируем все остальные файлы проекта
COPY . .
WORKDIR "/src"
RUN dotnet build "ExcelParser.csproj" -c Release -o /app/build

# Этап публикации (publish stage)
FROM build AS publish
RUN dotnet publish "ExcelParser.csproj" -c Release -o /app/publish

# Финальный этап (runtime image)
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ExcelParser.dll"]