using Infrastructure.Models;

namespace Infrastructure.Helpers;

public static class PopularMoviesHelper
{
    public static List<Movie> GetRandomMovies(IEnumerable<Movie> movies, int count = 3)
    {
        return movies
            .DistinctBy(movie => movie.Id)
            .OrderBy( _ => Random.Shared.Next())
            .Take(count)
            .ToList();
    }
}