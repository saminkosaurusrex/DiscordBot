namespace Application.Commands;

public record SlashCommandContext(
    string CommandName,
    string UserId,
    string ChannelId,
    IReadOnlyDictionary<string, object> Options,
    Func<string, bool, Task> Respond
    );