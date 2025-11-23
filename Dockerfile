# Etapa de build
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copiar el archivo de proyecto y restaurar dependencias
COPY ["LABO12/LABO12.csproj", "LABO12/"]
RUN dotnet restore "LABO12/LABO12.csproj"

# Copiar el resto de los archivos y compilar
COPY . .
WORKDIR "/src/LABO12"
RUN dotnet build "LABO12.csproj" -c Release -o /app/build

# Publicar la aplicación
FROM build AS publish
RUN dotnet publish "LABO12.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Etapa final - imagen de runtime
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app
EXPOSE 8080
COPY --from=publish /app/publish .

# Configurar Kestrel para escuchar en el puerto correcto
ENV ASPNETCORE_URLS=http://+:8080

ENTRYPOINT ["dotnet", "LABO12.dll"]
