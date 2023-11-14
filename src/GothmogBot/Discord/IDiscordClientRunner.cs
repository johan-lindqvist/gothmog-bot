using Discord.Rest;
using Discord.WebSocket;

namespace GothmogBot.Discord;

public interface IDiscordClientRunner
{
	Task RunAsync();
}
