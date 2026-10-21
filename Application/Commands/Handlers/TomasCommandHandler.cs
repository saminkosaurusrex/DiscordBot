using Application.Commands.Interfaces;

namespace Application.Commands.Handlers;

public class TomasCommandHandler : ISlashCommandHandler
{
    public string CommandName => "tomas";
    public string Description => "?";

    public Task HandleAsync(SlashCommandContext context, CancellationToken cancellationToken)
    {
        return context.Respond("Meow meow", false);
    }
}