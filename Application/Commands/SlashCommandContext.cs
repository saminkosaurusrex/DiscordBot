using Infrastructure.Models;

namespace Application.Commands;

public record SlashCommandContext(
    string CommandName,
    string UserId,
    string ChannelId,
    IReadOnlyDictionary<string, object> Options,
    Func<string, bool, Task> Respond,
    Func<Movie, Task> RespondWithMovie,
    Func<IEnumerable<Movie>, Task> RespondWithMovies
)
{
    public  T? GetOption<T>(
        SlashCommandContext context,
        string name)
        where T : struct
    {
        if (!context.Options.TryGetValue(name, out var value) ||
            value is null)
        {
            return null;
        }

        return (T)Convert.ChangeType(value, typeof(T));
    }
}