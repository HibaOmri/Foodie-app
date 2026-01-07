# Étape 1 : Build de l'application
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copier le fichier projet et restaurer les dépendances
COPY ["FoodieApp.csproj", "."]
RUN dotnet restore "FoodieApp.csproj"

# Copier tout le code source
COPY . .

# Builder l'application
RUN dotnet build "FoodieApp.csproj" -c Release -o /app/build

# Publier l'application
FROM build AS publish
RUN dotnet publish "FoodieApp.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Étape 2 : Image finale pour exécution
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app

# Exposer le port
EXPOSE 8080
EXPOSE 8081

# Copier les fichiers publiés
COPY --from=publish /app/publish .

# Définir le point d'entrée
ENTRYPOINT ["dotnet", "FoodieApp.dll"]
