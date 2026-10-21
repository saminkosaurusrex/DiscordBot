using System.Text.Json.Serialization;

namespace Infrastructure.Models;

public class Movie
{
    [JsonPropertyName("id")]
    public int Id { get; set; }
    [JsonPropertyName("original_title")]
    public required string Title { get; set; }
    [JsonPropertyName("overview")]
    public required string Overview { get; set; }
    [JsonPropertyName("vote_average")]
    public float VoteAverage { get; set; }
    [JsonPropertyName("poster_path")]
    public string? PosterPath { get; set; }
    [JsonPropertyName("release_date")]
    public string? ReleaseDate { get; set; }
}