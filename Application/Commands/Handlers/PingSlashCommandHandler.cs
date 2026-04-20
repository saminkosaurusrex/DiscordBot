using Application.Commands.Interfaces;

namespace Application.Commands.Handlers;

public class PingSlashCommandHandler : ISlashCommandHandler
{
    public string CommandName => "ping";
    public string Description => "replies with pong";

    public Task HandleAsync(SlashCommandContext context, CancellationToken cancellationToken)
    {
        return context.Respond("pong", false);
    }
}