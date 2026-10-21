using Application.Commands;
using Application.Commands.Interfaces;
using Discord;
using Discord.WebSocket;
using DiscordBot.Helpers;
using Infrastructure.Models;
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
    private readonly IMovieService _movieService;
    private IReadOnlyList<Genre>? _genres;

    public DiscordBotHostedService(DiscordSocketClient client,
        IEnumerable<ISlashCommandHandler> handlers,
        IOptions<DiscordOptions> options,
        ILogger<DiscordBotHostedService> logger,
        SlashCommandRegistrar registrar,
        IMovieService movieService)
    {
        _client = client;
        _handlers = handlers.ToDictionary(h=> h.CommandName, StringComparer.OrdinalIgnoreCase);
        _options = options.Value;
        _logger = logger;
        _registrar = registrar;
        _movieService = movieService;
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
        _client.AutocompleteExecuted += OnAutocompleteExecuted;

        
        await _client.LoginAsync(TokenType.Bot, _options.Token);
        await _client.StartAsync();
    }

    public  async Task StopAsync(CancellationToken cancellationToken)
    {
        _client.Log -= OnLog;
        _client.Ready -= OnReady;
        _client.AutocompleteExecuted -= OnAutocompleteExecuted;
        
        await _client.LogoutAsync();
        await _client.StopAsync();
    }

    private Task OnLog(LogMessage msg)
    {
        _logger.Log(
            msg.Severity switch
            {
                LogSeverity.Critical => LogLevel.Critical,
                LogSeverity.Error => LogLevel.Error,
                LogSeverity.Warning => LogLevel.Warning,
                LogSeverity.Info => LogLevel.Information,
                LogSeverity.Verbose => LogLevel.Debug,
                LogSeverity.Debug => LogLevel.Debug,
                _ => LogLevel.Information
            },
            msg.Exception,
            "[Discord] {Source}: {Message}",
            msg.Source,
            msg.Message
        );

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
            // ???
            ChannelId: interaction.ChannelId.ToString() ?? string.Empty,
            Options: options,
            Respond: (text, ephemeral) => interaction.RespondAsync(text, ephemeral: ephemeral),
            RespondWithMovie: movie =>
            {
                var embed = MovieEmbedBuilder.BuildMovie(movie);
                return interaction.RespondAsync(embed: embed);
            },
            RespondWithMovies: movies =>
            {
                var embed = MovieEmbedBuilder.BuildMovies(movies);
                return interaction.RespondAsync(embed: embed);
            }
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

    private async Task OnAutocompleteExecuted(SocketAutocompleteInteraction interaction)
    {
        if ((interaction.Data.CommandName != "movie" &&
             interaction.Data.CommandName != "popular") ||
            interaction.Data.Current.Name != "genre")
        {
            await interaction.RespondAsync(
                Array.Empty<AutocompleteResult>());
            return;
        }

        _genres ??= (await _movieService.GetTmdbMovieGenresAsync()).Genres;
        
        var input = interaction.Data.Current.Value?.ToString() ?? string.Empty;

        var result = _genres
            .Where(g => g.Name.Contains(input, StringComparison.InvariantCultureIgnoreCase))
            .Take(22)
            .Select(g => new AutocompleteResult(g.Name, g.Id.ToString()));
        
        await interaction.RespondAsync(result);
    }
}