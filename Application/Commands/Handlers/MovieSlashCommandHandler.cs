using Application.Commands.Interfaces;
using Discord;
using Infrastructure.Models;
using Infrastructure.Services;

namespace Application.Commands.Handlers;

public class MovieSlashCommandHandler : ISlashCommandHandler
{
    public string CommandName => "movie";

    public string Description => "Random movie suggestion by different parameters";
    
    private readonly IMovieService _movieService;

    public MovieSlashCommandHandler(IMovieService movieService)
    {
        _movieService = movieService;
    }

    public async Task HandleAsync(SlashCommandContext context, CancellationToken cancellationToken)
    {
        var options = new MovieSearchOptions
        {
            GenreId = int.Parse(context.Options["genre"]!.ToString()!),
            MinRating = GetOption<double>(context, "min_rating"),
            MinVoteCount = GetOption<int>(context, "min_votes"),
            YearFrom = GetOption<int>(context, "year_from"),
            YearTo = GetOption<int>(context, "year_to")
        };
        
        var response =  await _movieService.GetMovieAsync(options);
        var movies = response.Movies;
        if (movies.Count == 0)
        {
            await context.Respond("No movie match your preferences!", true);
        }

        var randIndex = Random.Shared.Next(0, movies.Count);
        var movie = movies[randIndex];
        
        await context.RespondWithMovie(movie);
    }
    
    private static T? GetOption<T>(
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