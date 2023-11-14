

using Discord.Interactions;
using Discord.WebSocket;
using Serilog;

namespace GothmogBot.Discord;

public class SlashCommandModule : InteractionModuleBase<SocketInteractionContext<SocketInteraction>>
{
	// RespondAsync() => Respond to the interaction
	// FollowupAsync() => Create a followup message for an interaction
	// ReplyAsync() => Send a message to the origin channel of the interaction
	// DeleteOriginalResponseAsync() => Delete the original interaction response

	[SlashCommand("sync-users-command", "Syncs MC users")]
	private async Task SyncUsers()
	{
		await RespondAsync($"Fake sync done");
	}

	[SlashCommand("fetch-games", "Compute the number of games played by each MC")]
	private async Task FetchGames()
	{
		await RespondAsync("Fake fetch done");
	}
}
