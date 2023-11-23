

using System.Collections.Immutable;
using System.Globalization;
using Discord.Interactions;
using Discord.WebSocket;
using GothmogBot.Services;

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
		await RespondAsync($"Fake sync done").ConfigureAwait(false);
	}

	[SlashCommand("fetch-games", "Compute the number of games played by each MC")]
	private async Task FetchGamesAsync()
	{
		await dotaService.GetDotaMatchAsync(CancellationToken.None).ConfigureAwait(false);

		await RespondAsync("Fake fetch done").ConfigureAwait(false);
	}

	[SlashCommand("add-me", "Use this to add Bulldog to your friends list")]
	private async Task AddMe()
	{
		// TODO: add correct url
		await RespondAsync("[Click here](http://localhost:5001/pair) to pair.").ConfigureAwait(false);
	}

	[SlashCommand("rorder", "Generate a random order")]
	private async Task RandomOrder(string prefix = "", string choices = "AdmiralBulldog Jitizm Sepitys Philaeux")
	{
		if (string.IsNullOrWhiteSpace(choices))
		{
			await RespondAsync($"Invalid value for parameter {nameof(choices)}").ConfigureAwait(false);
			return;
		}
		var choicesList = choices.Trim().Split(' ').Where(c => !string.IsNullOrWhiteSpace(c)).ToImmutableArray();

		if (string.IsNullOrWhiteSpace(prefix))
		{
			prefix = DateTime.Now.ToString("T", CultureInfo.CurrentCulture);
		}

		var random = new Random();
#pragma warning disable CA5394
		var words = choicesList.OrderBy(_ => random.Next()).ToImmutableArray();
#pragma warning restore CA5394

		await RespondAsync($"[{prefix}] {string.Join(" -> ", words)}").ConfigureAwait(false);
	}
}
