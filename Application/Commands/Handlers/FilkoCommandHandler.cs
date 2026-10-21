using Application.Commands.Interfaces;

namespace Application.Commands.Handlers;

public class FilkoCommandHandler : ISlashCommandHandler
{
    public string CommandName => "filko";
    public string Description => "?";

    public Task HandleAsync(SlashCommandContext context, CancellationToken cancellationToken)
    {
        return context.Respond("Galileo Galilei", false);
    }
}