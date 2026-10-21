using Application.Commands.Interfaces;

namespace Application.Commands.Handlers;

public class KeksSlashCommandHandler : ISlashCommandHandler
{
    public string CommandName => "keks";
    //dOb How hard is not to think?
    //  Random tag quote
    public string Description => "?";

    public Task HandleAsync(SlashCommandContext context, CancellationToken cancellationToken)
    {
        return context.Respond("Dudičko je velký pán", false);
    }
}