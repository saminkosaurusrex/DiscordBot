using System.Text.Json.Serialization;

namespace Infrastructure.Models;

public class MoviesResponse
{
    [JsonPropertyName("page")]
    public int Page { get; set; }
    [JsonPropertyName("results")]
    public List<Movie> Movies { get; set; } = new();
    [JsonPropertyName("total_pages")]
    public int TotalPages { get; set; }
    [JsonPropertyName("total_results")]
    public int TotalResults { get; set; }
}