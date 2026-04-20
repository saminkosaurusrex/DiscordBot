namespace Application.Commands.Interfaces;

public interface ISlashCommandHandler
{
    string CommandName { get; }
    string Description { get; }
    
    Task HandleAsync(SlashCommandContext context, CancellationToken cancellationToken);
}