namespace GothmogBot.Discord;

public sealed class DiscordOptions
{
	public const string SectionName = "DiscordOptions";

	public string DiscordApiToken { get; set; } = string.Empty;
}
