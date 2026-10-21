using Application.Commands.Interfaces;
using Discord;
using Infrastructure.Models;
using Infrastructure.Services;

namespace Application.Commands.Handlers;

public class MovieSlashCommandHandler(IMovieService movieService) : ISlashCommandHandler
{
    public string CommandName => "movie";

    public string Description => "Random movie suggestion by different parameters";

    public async Task HandleAsync(SlashCommandContext context, CancellationToken cancellationToken)
    {
        var options = new MovieSearchOptions
        {
            GenreId = int.Parse(context.Options["genre"]!.ToString()!),
            MinRating =context.GetOption<double>(context, "min_rating"),
            MinVoteCount = context.GetOption<int>(context, "min_votes"),
            YearFrom = context.GetOption<int>(context, "year_from"),
            YearTo = context.GetOption<int>(context, "year_to")
        };
        
        var response =  await movieService.GetMovieAsync(options, 0);
        var movies = response.Movies;
        if (movies.Count == 0)
        {
            await context.Respond("No movie match your preferences!", true);
        }

        var randIndex = Random.Shared.Next(0, movies.Count);
        var movie = movies[randIndex];
        
        await context.RespondWithMovie(movie);
    }
}