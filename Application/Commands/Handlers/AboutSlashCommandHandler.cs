using Application.Commands.Interfaces;

namespace Application.Commands.Handlers;

public class AboutSlashCommandHandler : ISlashCommandHandler
{
    public string CommandName => "about";
    public string Description => "Displays information about this discord BOT";

    public Task HandleAsync(SlashCommandContext context, CancellationToken cancellationToken)
    {
        return context.Respond("This bot does what @saminkosaurusrex taught it to do.", false);
    }
}