using Infrastructure.Models;

namespace Infrastructure.Services;

public interface IMovieService
{
    Task<MoviesResponse> GetMovieAsync(MovieSearchOptions options);
    Task<GenreResponse> GetTmdbMovieGenresAsync();
}