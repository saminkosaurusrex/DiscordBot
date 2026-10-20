
using System.Net.Http.Headers;
using System.Text.Json;
using Infrastructure.Models;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Options;

namespace Infrastructure.Services;

public class TmdbService : IMovieService
{
    private readonly HttpClient _httpClient;
    private readonly TmdbOptions  _options;
    private const string MOVIEURL = "https://api.themoviedb.org/3/";
        
    public TmdbService(HttpClient httpClient, IOptions<TmdbOptions> options)
    {
        _httpClient = httpClient;
        _httpClient.DefaultRequestHeaders.Accept.Add( new MediaTypeWithQualityHeaderValue("application/json"));
        _httpClient.BaseAddress = new Uri(MOVIEURL);
        _options = options.Value;
    }


    public async Task<GenreResponse> GetTmdbMovieGenresAsync()
    {
        var url = QueryHelpers.AddQueryString(
            "genre/movie/list",
            new Dictionary<string, string?>
            {
                ["api_key"] = _options.ApiKey
            });
        using var response =
            await _httpClient.GetAsync(url);
        response.EnsureSuccessStatusCode();
        
        var json = await response.Content.ReadAsStringAsync();
        var result = JsonSerializer.Deserialize<GenreResponse>(json);
        
        if (result == null)
            throw new Exception("No genre found");
        return result;
    }

    public async Task<MoviesResponse> GetMovieAsync(MovieSearchOptions options)
    {
        var queryParams = new Dictionary<string, string?>
        {
            ["with_genres"] = options.GenreId.ToString(),
            ["api_key"] = _options.ApiKey,
            ["vote_average.gte"] = options.MinRating?.ToString(
                System.Globalization.CultureInfo.InvariantCulture),
            ["vote_count.gte"] = options.MinVoteCount?.ToString(),
            ["primary_release_date.gte"] = options.YearFrom.HasValue
                ? $"{options.YearFrom}-01-01"
                : null,
            ["primary_release_date.lte"] = options.YearTo.HasValue
                ? $"{options.YearTo}-12-31"
                : null
        };

        var url = QueryHelpers.AddQueryString("discover/movie", queryParams);
        using var response =
            await _httpClient.GetAsync(url);
        response.EnsureSuccessStatusCode();
        
        var json = await response.Content.ReadAsStringAsync();
        var result = JsonSerializer.Deserialize<MoviesResponse>(json);
        
        if (result == null)
            throw new Exception("No genre found");
        return result;
    }
}