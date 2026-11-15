# Етап 1: Збірка (Build)
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Копіюємо файл проєкту та відновлюємо залежності
COPY ["MyWebAPI.csproj", "./"]
RUN dotnet restore "MyWebAPI.csproj"

# Копіюємо весь інший код та публікуємо
COPY . .
RUN dotnet publish "MyWebAPI.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Етап 2: Виконання (Run)
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app

# Налаштовуємо порти (в .NET 8 за замовчуванням використовується 8080)
EXPOSE 8080
ENV ASPNETCORE_HTTP_PORTS=8080

COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "MyWebAPI.dll"]