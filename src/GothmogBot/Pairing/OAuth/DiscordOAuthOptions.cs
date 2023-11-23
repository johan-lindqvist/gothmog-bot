namespace GothmogBot.Pairing.OAuth;

public sealed class DiscordOAuthOptions
{
	public const string SectionName = "DiscordOAuthOptions";

	public string ClientId { get; set; } = string.Empty;

	public string ClientSecret { get; set; } = string.Empty;

#pragma warning disable CA1056 // URI-like properties should not be strings
	public string RedirectUrl { get; set; } = string.Empty;
#pragma warning restore CA1056 // URI-like properties should not be strings
}
