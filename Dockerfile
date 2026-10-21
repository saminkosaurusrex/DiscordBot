
# Build stage
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Copy solution and project files
COPY DiscordBot.sln ./
COPY Application/Application.csproj Application/
COPY Domain/Domain.csproj Domain/
COPY Infrastructure/Infrastructure.csproj Infrastructure/
COPY DiscordBot/DiscordBot.csproj DiscordBot/

# Restore dependencies
RUN dotnet restore DiscordBot/DiscordBot.csproj

# Copy source code
COPY Application/ Application/
COPY Domain/ Domain/
COPY Infrastructure/ Infrastructure/
COPY DiscordBot/ DiscordBot/

# Publish application
RUN dotnet publish DiscordBot/DiscordBot.csproj \
    -c Release \
    -o /app/publish \
    --no-restore

# Runtime stage
FROM mcr.microsoft.com/dotnet/runtime:10.0 AS final
WORKDIR /app

COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "DiscordBot.dll"]

