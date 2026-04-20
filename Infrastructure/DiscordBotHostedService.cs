using Application.Commands;
using Application.Commands.Interfaces;
using Discord;
using Discord.WebSocket;
using Infrastructure.Services;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Infrastructure;

public sealed class DiscordBotHostedService : IHostedService
{
    private readonly DiscordSocketClient _client;
    private readonly DiscordOptions _options;
    private readonly IReadOnlyDictionary<string, ISlashCommandHandler> _handlers;
    private readonly ILogger<DiscordBotHostedService> _logger;
    private readonly SlashCommandRegistrar _registrar;

    public DiscordBotHostedService(DiscordSocketClient client,
        IEnumerable<ISlashCommandHandler> handlers,
        IOptions<DiscordOptions> options,
        ILogger<DiscordBotHostedService> logger,
        SlashCommandRegistrar registrar)
    {
        _client = client;
        _handlers = handlers.ToDictionary(h=> h.CommandName, StringComparer.OrdinalIgnoreCase);
        _options = options.Value;
        _logger = logger;
        _registrar = registrar;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(_options.Token))
        {
            throw new InvalidOperationException("Discord token is not configured");
        }

        _client.Log += OnLog;
        _client.Ready += OnReady;
        _client.SlashCommandExecuted += OnSlashCommandExecuted;

        
        await _client.LoginAsync(TokenType.Bot, _options.Token);
        await _client.StartAsync();
    }

    public  async Task StopAsync(CancellationToken cancellationToken)
    {
        _client.Log -= OnLog;
        _client.Ready -= OnReady;
        
        await _client.LogoutAsync();
        await _client.StopAsync();
    }

    private Task OnLog(LogMessage msg)
    {
        _logger.LogInformation("[Discord] {Message}", msg.Message ?? msg.Exception?.Message);
        return Task.CompletedTask;
    }

    private async Task OnReady()
    {
        await _registrar.RegisterAsync(_options.DevelopmentGuildId);
        _logger.LogInformation("Bot is connected as {User}", _client.CurrentUser);
        
    }

    private async Task OnSlashCommandExecuted(SocketSlashCommand interaction)
    {
        if (!_handlers.TryGetValue(interaction.CommandName, out var handler))
        {
            await interaction.RespondAsync("Unknown command", ephemeral: true);
            return;
        }
        
        var options = interaction.Data.Options
            .ToDictionary(o=>o.Name, o=>o.Value);

        var context = new SlashCommandContext(
            CommandName: interaction.CommandName,
            UserId: interaction.User.Id.ToString(),
            ChannelId: interaction.ChannelId?.ToString(),
            Options: options,
            Respond: (text, ephemeral) => interaction.RespondAsync(text, ephemeral: ephemeral)
            );
        
        try
        {
            await handler.HandleAsync(context, CancellationToken.None);
            
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error executing slash command {Command}", interaction.Data.Name);
            if (!interaction.HasResponded)
                //fallback
                await interaction.RespondAsync("Something went wrong.", ephemeral: true);
        }
    }
}