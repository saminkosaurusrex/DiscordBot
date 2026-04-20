namespace Infrastructure;

public sealed class DiscordOptions
{
    public const string SectionName = "Discord";

    public string Token { get; set; } = string.Empty;
    public ulong? DevelopmentGuildId { get; set; }
}