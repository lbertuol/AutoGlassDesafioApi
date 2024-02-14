FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["AutoGlassDesafioApi.Presentation/AutoGlassDesafioApi.Presentation.csproj", "AutoGlassDesafioApi.Presentation/"]
RUN dotnet restore "./AutoGlassDesafioApi.Presentation/./AutoGlassDesafioApi.Presentation.csproj"
COPY . .
WORKDIR "/src/AutoGlassDesafioApi.Presentation"
RUN dotnet build "./AutoGlassDesafioApi.Presentation.csproj" -c %BUILD_CONFIGURATION% -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./AutoGlassDesafioApi.Presentation.csproj" -c %BUILD_CONFIGURATION% -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "AutoGlassDesafioApi.Presentation.dll"]