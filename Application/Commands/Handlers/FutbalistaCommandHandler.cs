using Application.Commands.Interfaces;

namespace Application.Commands.Handlers;

public class FutbalistaCommandHandler : ISlashCommandHandler
{
    public string CommandName => "futbalista";
    public string Description => "?";
    public Task HandleAsync(SlashCommandContext context, CancellationToken cancellationToken)
    {
        return context.Respond("ROBLOX final BOSS", false);
    }
}