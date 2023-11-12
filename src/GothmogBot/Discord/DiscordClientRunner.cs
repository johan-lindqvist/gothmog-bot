using Discord;
using Discord.Rest;
using Discord.WebSocket;
using Microsoft.Extensions.Options;
using Serilog;

namespace GothmogBot.Discord;

public sealed class DiscordClientRunner : IDiscordClientRunner
{
	private readonly IOptions<DiscordOptions> discordOptions;

	public DiscordClientRunner(
		DiscordSocketClient discordSocketClient,
		DiscordRestClient discordRestClient,
		IOptions<DiscordOptions> discordOptions)
	{
		this.discordOptions = discordOptions;
		DiscordSocketClient = discordSocketClient;
		DiscordRestClient = discordRestClient;
	}

	public DiscordSocketClient DiscordSocketClient { get; init; }

	public DiscordRestClient DiscordRestClient { get; init; }

	public async Task RunAsync()
	{
		DiscordSocketClient.Log += DiscordLogger.LogAsync;

		await DiscordSocketClient.LoginAsync(TokenType.Bot, discordOptions.Value.DiscordApiToken);
		await DiscordSocketClient.StartAsync();

		DiscordRestClient.Log += DiscordLogger.LogAsync;

		await DiscordRestClient.LoginAsync(TokenType.Bot, discordOptions.Value.DiscordApiToken);

		Log.Information("Discord Bot started");
	}
}
