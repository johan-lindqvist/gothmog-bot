

using Discord.Interactions;
using Discord.WebSocket;
using GothmogBot.Services;
using Serilog;

namespace GothmogBot.Discord;

public sealed class SlashCommandModule : InteractionModuleBase<SocketInteractionContext<SocketInteraction>>
{
	private readonly DotaService dotaService;
	// RespondAsync() => Respond to the interaction
	// FollowupAsync() => Create a followup message for an interaction
	// ReplyAsync() => Send a message to the origin channel of the interaction
	// DeleteOriginalResponseAsync() => Delete the original interaction response

	public SlashCommandModule(DotaService dotaService)
	{
		this.dotaService = dotaService;
	}

	[SlashCommand("sync-users-command", "Syncs MC users")]
	private async Task SyncUsersAsync()
	{
		await RespondAsync($"Fake sync done");
	}

	[SlashCommand("fetch-games", "Compute the number of games played by each MC")]
	private async Task FetchGamesAsync()
	{
		await dotaService.GetDotaMatchAsync(CancellationToken.None);

		await RespondAsync("Fake fetch done");
	}
}
