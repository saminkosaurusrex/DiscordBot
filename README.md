# 🎬 Discord Movie Bot

A Discord bot built with **C# and .NET 10** that brings movie discovery and information directly into Discord. It integrates with [The Movie Database (TMDB)](https://www.themoviedb.org/) to retrieve movie information and provides a collection of slash commands.

The application is containerized with Docker and hosted on **Azure Container Apps**, with CI/CD automation using Azure DevOps.

## ✨ Features

- **Movie information** — retrieve movie details using TMDB.
- **Popular movies** — discover currently popular movies.
- **Slash commands** — interact with the bot directly through Discord.
- **Health and connectivity checks** — check whether the bot is responding.
- **Modular architecture** — separate domain logic, application logic, infrastructure, and the Discord entry point.
- **Containerized deployment** — package the application as a Docker image.
- **Cloud hosting** — run the bot on Azure Container Apps.
- **Automated delivery** — use Azure DevOps pipelines to build and deploy new versions.

## 🛠️ Tech Stack

| Technology | Purpose |
|---|---|
| C# / .NET 10 | Application development |
| Discord.Net | Discord API integration |
| TMDB API | Movie data and discovery |
| Docker | Containerization |
| Azure Container Apps | Cloud hosting |
| Azure Container Registry | Container image storage |
| Azure DevOps | CI/CD automation |
| GitHub | Source code management |

## 🏗️ Architecture

The solution is organized into separate projects to keep responsibilities isolated.

```text
DiscordMovieBot/
├── DiscordBot/       # Discord integration and application entry point
├── Application/      # Application logic and use cases
├── Infrastructure/   # External services and TMDB integration
├── Domain/           # Core domain models and logic
├── Dockerfile
└── azure-pipelines.yml
```

The exact repository structure may vary depending on the current implementation.

### CI/CD workflow

The deployment pipeline follows this workflow:

```text
GitHub Repository
       |
       v
Azure DevOps Pipeline
       |
       v
Build Docker Image
       |
       v
Azure Container Registry
       |
       v
Azure Container Apps
```

The pipeline automates the delivery process, reducing the need for manual builds and deployments.

## 🤖 Available Commands

| Command       | Description                         |
|---------------|-------------------------------------|
| `/about`      | Display information about the bot.  |
| `/movie`      | Retrieve information about a movie. |
| `/popular`    | Discover popular movies.            |
| `/ping`       | Check bot responsiveness.           |
| `/keks`       | Fun command for friend.             |
| `/tomas`      | Fun command for friend.             |
| `/filko`      | Custom command for friend.                    |
| `/futbalista` | Custom command for friend.                    |

## 🚀 Running Locally

### Prerequisites

- [.NET 10 SDK](https://dotnet.microsoft.com/download)
- [Discord application and bot](https://discord.com/developers/applications)
- [TMDB API key](https://www.themoviedb.org/settings/api)

### 1. Clone the repository

```bash
git clone https://github.com/saminkosaurusrex/DiscordBot.git
cd DiscordBot
```

Replace `DiscordBot` with the actual repository name.

### 2. Configure secrets

Set the required configuration values using .NET User Secrets or environment variables.

For local development, initialize User Secrets in the startup project:

```bash
cd DiscordBot
dotnet user-secrets init
dotnet user-secrets set "Discord:Token" "YOUR_DISCORD_BOT_TOKEN"
dotnet user-secrets set "Tmdb:ApiKey" "YOUR_TMDB_API_KEY"
```

Never commit real bot tokens, API keys, or other credentials to source control.

### 3. Run the application

From the repository root:

```bash
dotnet run --project DiscordBot
```

Make sure the Discord bot has been invited to your server with the required permissions.

## 🐳 Docker

Build the Docker image from the repository root:

```bash
docker build -t discord-movie-bot .
```

Run the container with the required environment variables:

```bash
docker run --rm \
  -e Discord__Token="YOUR_DISCORD_BOT_TOKEN" \
  -e Tmdb__ApiKey="YOUR_TMDB_API_KEY" \
  discord-movie-bot
```

## ☁️ Deployment

The bot is designed to run on Azure Container Apps, with Docker images stored in Azure Container Registry.

Production configuration and credentials are managed outside the source code using Azure's configuration and secret-management capabilities.

The Azure DevOps pipeline automates the build and deployment workflow. See the pipeline configuration in the repository for the implementation details.

## 🔐 Security

- Keep Discord tokens and TMDB API keys out of source control.
- Use secret variables and secure service connections in CI/CD.
- Store production credentials in Azure Container Apps secrets.
- Grant deployment identities only the permissions they need.

## 📄 License

This project is licensed under the [MIT License](https://opensource.org/license/mit/).

You are free to use, copy, modify, merge, publish, distribute, sublicense, and sell copies of the software, subject to the conditions of the license.

See the [LICENSE](LICENSE) file for the full license text.