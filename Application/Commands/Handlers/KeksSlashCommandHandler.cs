using Application.Commands.Interfaces;

namespace Application.Commands.Handlers;

public class KeksSlashCommandHandler : ISlashCommandHandler
{
    public string CommandName => "keks";
    public string Description => "?";

    public Task HandleAsync(SlashCommandContext context, CancellationToken cancellationToken)
    {
        return context.Respond("Dudičko je velký pán", false);
    }
}