namespace Infrastructure.Models;

public class MovieSearchOptions
{
    public int GenreId { get; set; }
    public double? MinRating { get; set; }
    public int? MinVoteCount { get; set; }
    public int? YearFrom { get; set; }
    public int? YearTo { get; set; }
}