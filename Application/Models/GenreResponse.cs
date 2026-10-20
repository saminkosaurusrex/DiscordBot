using System.Text.Json.Serialization;

namespace Infrastructure.Models;

public class GenreResponse
{
    [JsonPropertyName("genres")]
    public List<Genre> Genres { get; set; } = new();
}