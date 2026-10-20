using Discord;
using Infrastructure.Models;

namespace DiscordBot.Helpers;

public static class MovieEmbedBuilder
{
    public static Embed Build(Movie movie)
    {
        var description = string.IsNullOrWhiteSpace(movie.Overview)
            ? "No description available."
            : movie.Overview.Length > 1000
                ? movie.Overview[..1000] + "..."
                : movie.Overview;
        
        var builder = new EmbedBuilder()
            .WithTitle(movie.Title)
            .WithDescription(description)
            .WithColor(Color.DarkBlue)
            .WithUrl($"https://www.themoviedb.org/movie/{movie.Id}")
            .AddField("⭐ Rating", $"{movie.VoteAverage:F1}/10", true)
            .AddField(
                "🎬 STREAMEX",
                $"[Watch / Search](https://www.streamex.hn/search?query={Uri.EscapeDataString(movie.Title)})",
                true)
            .WithFooter("Movie data provided by TMDb");
        
        if (!string.IsNullOrWhiteSpace(movie.ReleaseDate))
        {
            builder.AddField("📅 Release date", movie.ReleaseDate, true);
        }

        if (!string.IsNullOrWhiteSpace(movie.PosterPath))
        {
            builder.WithImageUrl(
                $"https://image.tmdb.org/t/p/w500{movie.PosterPath}");
        }

        return builder.Build();
    }
}