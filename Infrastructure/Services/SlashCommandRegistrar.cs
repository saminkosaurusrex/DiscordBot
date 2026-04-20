using Application.Commands.Interfaces;
using Discord;
using Discord.WebSocket;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Services;

public sealed class SlashCommandRegistrar
{
    private readonly DiscordSocketClient _client;
    private readonly IEnumerable<ISlashCommandHandler> _handlers;
    private readonly ILogger<SlashCommandRegistrar> _logger;

    public SlashCommandRegistrar(DiscordSocketClient client, IEnumerable<ISlashCommandHandler> handlers,
        ILogger<SlashCommandRegistrar> logger)
    {
        _client = client;
        _handlers = handlers;
        _logger = logger;
    }

    public async Task RegisterAsync(ulong? developmentGuildId)
    {
        var commands = _handlers.Select(h =>
            new SlashCommandBuilder()
                .WithName(h.CommandName)
                .WithDescription(h.Description)
                . Build())
            .ToArray();

        if (developmentGuildId.HasValue)
        {
            var guild = _client.GetGuild(developmentGuildId.Value);
            if (guild is null)
            {
                _logger.LogWarning("Development guild {GuildId} not found", developmentGuildId);
                return;
            }
            
            await guild.BulkOverwriteApplicationCommandAsync(commands);
            _logger.LogInformation("Registered {Count} commands to guild {Guild}", commands.Length, guild.Name);
            
        }
        else
        {
            await _client.BulkOverwriteGlobalApplicationCommandsAsync(commands);
            _logger.LogInformation("Registered {Count} commands to global", commands.Length);
        }
    }
}