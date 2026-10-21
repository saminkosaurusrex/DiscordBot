using Infrastructure.Models;

namespace Infrastructure.Services;

public interface IMovieService
{
    Task<MoviesResponse> GetMovieAsync(MovieSearchOptions options, int randomPage);
    Task<GenreResponse> GetTmdbMovieGenresAsync();
    Task<MoviesResponse> GetPopularMovies(MovieSearchOptions options);
}