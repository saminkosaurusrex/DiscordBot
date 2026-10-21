using Application.Commands.Interfaces;
using Infrastructure.Models;
using Infrastructure.Services;

namespace Application.Commands.Handlers;

public class PopularMoviesHandler(IMovieService movieService) : ISlashCommandHandler
{
    public string CommandName => "popular";
    public string Description => "3 Popular movies, based on params";

    public async Task HandleAsync(SlashCommandContext context, CancellationToken cancellationToken)
    {
        var options = new MovieSearchOptions
        {
            GenreId = int.Parse(context.Options["genre"].ToString()!),
            MinRating = context.GetOption<double>(context, "min_rating"),
            MinVoteCount = context.GetOption<int>(context, "min_votes"),
            YearFrom = context.GetOption<int>(context, "year_from"),
            YearTo = context.GetOption<int>(context, "year_to")
        };
        
        var response = await movieService.GetPopularMovies(options);
        var movies = response.Movies;
        if (movies.Count == 0)
        {
            await context.Respond("No movie match your preferences!", true);
        }
        await context.RespondWithMovies(GetRandomMovies(movies));
    }
    
    private static List<Movie> GetRandomMovies(IEnumerable<Movie> movies, int count = 3)
    {
        return movies
            .DistinctBy(movie => movie.Id)
            .OrderBy( _ => Random.Shared.Next())
            .Take(count)
            .ToList();
    }
}