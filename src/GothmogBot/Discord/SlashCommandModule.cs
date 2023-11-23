

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
		string realPrefix = prefix;
		if (realPrefix.Length == 0)
		{
			TimeZoneInfo cetTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Central European Standard Time");
			DateTime currentTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, cetTimeZone);
			realPrefix = currentTime.ToString("HH:mm:ss");
		}

		string[] words = choices.Split(' ');
		Random random = new Random();
		words = words.OrderBy(w => random.Next()).ToArray();

		await RespondAsync(realPrefix + " - " + string.Join(" ", words)).ConfigureAwait(false);
	}
}
