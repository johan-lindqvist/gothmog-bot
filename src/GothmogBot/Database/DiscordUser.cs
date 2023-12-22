namespace GothmogBot.Database;

public sealed record DiscordUser
{
    public long Id { get; set; }

    public ulong DiscordId { get; set; }

    public string DiscordUsername { get; set; }

    public long Points { get; set; }

    public long HourlyPoints { get; set; }

    public long DailyPoints { get; set; }

    public long WeeklyPoints { get; set; }
}