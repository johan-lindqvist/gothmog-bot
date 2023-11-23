using System.Text.Json.Serialization;

namespace GothmogBot.Pairing.OAuth;

public sealed record DiscordAuthResponse(
	[property: JsonPropertyName("access_token")]
	string AccessToken,
	[property: JsonPropertyName("token_type")]
	string TokenType,
	[property: JsonPropertyName("expires_in")]
	int ExpiresIn,
	[property: JsonPropertyName("refresh_token")]
	string RefreshToken,
	[property: JsonPropertyName("scope")]
	string Scope);
