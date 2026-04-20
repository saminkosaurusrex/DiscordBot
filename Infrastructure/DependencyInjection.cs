using Discord;
using Discord.WebSocket;
using Infrastructure.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddOptions<DiscordOptions>()
            .Bind(configuration.GetSection(DiscordOptions.SectionName))
            .Validate(o => !string.IsNullOrWhiteSpace(o.Token), "Discord token is required.")
            .ValidateOnStart();
        
        services.AddSingleton(new DiscordSocketConfig
        {
            GatewayIntents = GatewayIntents.Guilds |
                             GatewayIntents.GuildMembers |
                             GatewayIntents.MessageContent,
            LogLevel = LogSeverity.Info,
        });

        services.AddSingleton<SlashCommandRegistrar>();
        services.AddSingleton<DiscordSocketClient>();
        services.AddHostedService<DiscordBotHostedService>();
        
        return services;
    }
}