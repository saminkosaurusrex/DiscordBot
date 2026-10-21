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
        {
            var builder = new SlashCommandBuilder()
                .WithName(h.CommandName)
                .WithDescription(h.Description);

            if (h.CommandName == "movie" || h.CommandName == "popular")
            {
                builder.AddOption(
                    name: "genre",
                    type: ApplicationCommandOptionType.String,
                    description: "Genre of the movie",
                    isRequired: true,
                    isAutocomplete: true);

                builder.AddOption(
                    name: "min_rating",
                    type: ApplicationCommandOptionType.Number,
                    description: "Minimum movie rating (0-10)",
                    isRequired: false,
                    minValue: 0,
                    maxValue: 10);

                builder.AddOption(
                    name: "min_votes",
                    type: ApplicationCommandOptionType.Integer,
                    description: "Minimum number of votes",
                    isRequired: false,
                    minValue: 0);

                builder.AddOption(
                    name: "year_from",
                    type: ApplicationCommandOptionType.Integer,
                    description: "Earliest release year",
                    isRequired: false,
                    minValue: 1870,
                    maxValue: 2100);

                builder.AddOption(
                    name: "year_to",
                    type: ApplicationCommandOptionType.Integer,
                    description: "Latest release year",
                    isRequired: false,
                    minValue: 1870,
                    maxValue: 2100);
            }

            return builder.Build();
        }).ToArray();
            

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