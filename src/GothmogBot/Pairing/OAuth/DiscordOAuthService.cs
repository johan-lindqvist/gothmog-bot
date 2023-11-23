using Microsoft.Extensions.Options;
using Serilog;

namespace GothmogBot.Pairing.OAuth;

public sealed class DiscordOAuthService
{
	private readonly IOptions<DiscordOAuthOptions> discordOAuthOptions;
	private readonly IHttpClientFactory httpClientFactory;

	public DiscordOAuthService(
		IOptions<DiscordOAuthOptions> discordOAuthOptions,
		IHttpClientFactory httpClientFactory)
	{
		this.discordOAuthOptions = discordOAuthOptions;
		this.httpClientFactory = httpClientFactory;
	}

	public async Task<(bool Success, string? AccessToken)> GetAccessTokenAsync(string code)
	{
		using var client = httpClientFactory.CreateClient();
		using var formContent = new FormUrlEncodedContent(new[]
		{
			new KeyValuePair<string, string>("code", code),
			new KeyValuePair<string, string>("grant_type", "authorization_code"),
			new KeyValuePair<string, string>("redirect_uri", discordOAuthOptions.Value.RedirectUrl),
			new KeyValuePair<string, string>("client_id", discordOAuthOptions.Value.ClientId),
			new KeyValuePair<string, string>("client_secret", discordOAuthOptions.Value.ClientSecret),
		});

		formContent.Headers.Clear();
		formContent.Headers.Add("Content-Type", "application/x-www-form-urlencoded");

		using var response = await client.PostAsync(new Uri("https://discord.com/api/oauth2/token"), formContent).ConfigureAwait(false);

		if (!response.IsSuccessStatusCode)
		{
			Log.Warning("Failed to get oauth token for user, status code {StatusCode}", response.StatusCode);
			return (false, null);
		}

		var authResponse = await response.Content.ReadFromJsonAsync<DiscordAuthResponse>().ConfigureAwait(false);

		if (authResponse == null)
		{
			Log.Warning("Unable to parse Discord OAuth response. Response: {AuthResponse}", await response.Content.ReadAsStringAsync().ConfigureAwait(false));
			return (false, null);
		}

		return (true, authResponse.AccessToken);
	}
}
