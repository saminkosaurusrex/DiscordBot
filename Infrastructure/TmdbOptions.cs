namespace Infrastructure;

public sealed class TmdbOptions
{
    public const string SectionName = "Tmdb";
    public string ApiKey { get; set; } =  string.Empty;
}