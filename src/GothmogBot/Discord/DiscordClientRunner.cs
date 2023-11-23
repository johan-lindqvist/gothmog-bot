using Discord;
using Discord.Rest;
using Discord.WebSocket;
using Microsoft.Extensions.Options;
using Serilog;

namespace GothmogBot.Discord;

public sealed class DiscordClientRunner : IDiscordClientRunner
{
	private readonly DiscordSocketClient discordSocketClient;
	private readonly DiscordRestClient discordRestClient;
	private readonly InteractionHandler interactionHandler;
	private readonly IOptions<DiscordOptions> discordOptions;

	public DiscordClientRunner(
		DiscordSocketClient discordSocketClient,
		DiscordRestClient discordRestClient,
		InteractionHandler interactionHandler,
		IOptions<DiscordOptions> discordOptions)
	{
		this.discordSocketClient = discordSocketClient;
		this.discordRestClient = discordRestClient;
		this.interactionHandler = interactionHandler;
		this.discordOptions = discordOptions;
	}

	public async Task RunAsync()
	{
		discordSocketClient.Log += DiscordLogger.LogAsync;
		Log.Information("Discord Socket Client started");
		await discordSocketClient.LoginAsync(TokenType.Bot, discordOptions.Value.DiscordApiToken).ConfigureAwait(false);
		await discordSocketClient.StartAsync().ConfigureAwait(false);

		discordRestClient.Log += DiscordLogger.LogAsync;
		Log.Information("Discord Rest Client started");
		await discordRestClient.LoginAsync(TokenType.Bot, discordOptions.Value.DiscordApiToken).ConfigureAwait(false);

		await interactionHandler.InitializeAsync().ConfigureAwait(false);
	}
}
