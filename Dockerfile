FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ["EY.DueDiligenceScreening.API/EY.DueDiligenceScreening.API.csproj", "EY.DueDiligenceScreening.API/"]
RUN dotnet restore "EY.DueDiligenceScreening.API/EY.DueDiligenceScreening.API.csproj"

COPY . .
WORKDIR "/src/EY.DueDiligenceScreening.API"
RUN dotnet publish "EY.DueDiligenceScreening.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/playwright/dotnet:v1.49.0-jammy AS final
WORKDIR /app

ENV PLAYWRIGHT_BROWSERS_PATH=/ms-playwright

COPY --from=build /app/publish .

ENV ASPNETCORE_URLS=http://+:80
ENV ASPNETCORE_ENVIRONMENT=Production

EXPOSE 80

ENTRYPOINT ["dotnet", "EY.DueDiligenceScreening.API.dll"]