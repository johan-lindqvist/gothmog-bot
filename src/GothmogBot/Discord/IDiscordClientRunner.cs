using Discord.Rest;
using Discord.WebSocket;

namespace GothmogBot.Discord;

public interface IDiscordClientRunner
{
	Task RunAsync();

	public DiscordSocketClient DiscordSocketClient { get; init; }

	public DiscordRestClient DiscordRestClient { get; init; }
}
