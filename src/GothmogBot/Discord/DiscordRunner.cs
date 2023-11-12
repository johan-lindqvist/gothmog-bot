using Discord;
using Discord.WebSocket;
using Microsoft.Extensions.Options;
using Serilog;

namespace GothmogBot.Discord;

public sealed class DiscordRunner : IDiscordRunner
{
	private readonly DiscordSocketClient client;
	private readonly IOptions<DiscordOptions> discordOptions;

	public DiscordRunner(
		DiscordSocketClient client,
		IOptions<DiscordOptions> discordOptions)
	{
		this.client = client;
		this.discordOptions = discordOptions;
	}

	public async Task RunAsync()
	{
		client.Log += DiscordLogger.LogAsync;

		await client.LoginAsync(TokenType.Bot, discordOptions.Value.DiscordApiToken);
		await client.StartAsync();

		Log.Information("Discord Bot started");
	}
}
