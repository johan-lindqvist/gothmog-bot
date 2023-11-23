using Discord;
using Discord.Rest;
using GothmogBot.Discord;
using GothmogBot.Pairing.OAuth;
using Serilog;

namespace GothmogBot.Pairing;

public sealed class PairingService
{
	private const string SteamConnectionIdentifier = "steam";
	private const string TwitchConnectionIdentifier = "twitch";

	private readonly DiscordOAuthService discordOAuthService;
	private readonly DiscordRestClient restClient;

	public PairingService(
		DiscordOAuthService discordOAuthService,
		DiscordRestClient restClient)
	{
		this.discordOAuthService = discordOAuthService;
		this.restClient = restClient;

		this.restClient.Log += DiscordLogger.LogAsync;
	}

	public async Task<bool> PairUserAsync(string code)
	{
		Log.Information("Pairing user");

		var (success, accessToken) = await discordOAuthService.GetAccessTokenAsync(code).ConfigureAwait(false);

		if (!success)
		{
			Log.Warning("Failed to get access token for user with code {Code}", code);
			return false;
		}

		// TODO: store the user connections in the database?
		var userConnections = await GetConnectionsAsync(accessToken!).ConfigureAwait(false);

		return true;
	}

	private async Task<UserConnections> GetConnectionsAsync(string accessToken)
	{
		await restClient.LoginAsync(TokenType.Bearer, $"{accessToken}").ConfigureAwait(false);

		Log.Information("Getting connections for user");
		var restConnections = await restClient.GetConnectionsAsync().ConfigureAwait(false);

		var steamRestConnection = restConnections.FirstOrDefault(c => c.Type == SteamConnectionIdentifier);
		var steamConnection = steamRestConnection is null
			? null
			: new Connection(ConnectionType.Steam, steamRestConnection.Id, steamRestConnection.Name);

		var twitchRestConnection = restConnections.FirstOrDefault(c => c.Type == TwitchConnectionIdentifier);
		var twitchConnection = twitchRestConnection is null
			? null
			: new Connection(ConnectionType.Twitch, twitchRestConnection.Id, twitchRestConnection.Name);

		Log.Information("Finished getting connections for user. Connections: {SteamConnection} {TwitchConnection}", steamConnection, twitchConnection);
		return new UserConnections(steamConnection, twitchConnection);
	}
}
