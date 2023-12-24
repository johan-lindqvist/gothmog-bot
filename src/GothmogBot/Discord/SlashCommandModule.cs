

using System.Collections.Immutable;
using System.Globalization;
using Discord;
using Discord.Interactions;
using Discord.WebSocket;
using GothmogBot.Database;
using GothmogBot.Services;
using Microsoft.EntityFrameworkCore;

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

	[SlashCommand("points", "Get current points")]
	private async Task GetPoints()
	{
		using var db = new ApplicationDbContext();

		var discordUser = Context.User;

		var user = await db.DiscordUsers.FirstOrDefaultAsync(u => u.DiscordId == discordUser.Id).ConfigureAwait(false);

		if (user == null)
		{
			var guildUser = Context.Guild.GetUser(discordUser.Id);
			if (guildUser == null)
			{
				await RespondAsync($"Invalid value for parameter {nameof(user)}").ConfigureAwait(false);
				return;
			}

			user = new DiscordUser
			{
				DiscordId = discordUser.Id,
				DiscordUsername = guildUser.Username,
				Points = 0,
			};

			db.Add(user);
		}

		await RespondAsync($"{user.Points}").ConfigureAwait(false);
	}

	[SlashCommand("set-points", "Set points for a user")]
	[RequireRole(roleId: DiscordConstants.DiscordModsRoleId, Group = "Permission")]
	private async Task SetPoints(IUser discordUser, long points)
	{
		using var db = new ApplicationDbContext();

		var user = await db.DiscordUsers.FirstOrDefaultAsync(u => u.DiscordId == discordUser.Id).ConfigureAwait(false);

		if (user == null)
		{
			var guildUser = Context.Guild.GetUser(discordUser.Id);
			if (guildUser == null)
			{
				await RespondAsync($"Invalid value for parameter {nameof(user)}").ConfigureAwait(false);
				return;
			}

			user = new DiscordUser
			{
				DiscordId = discordUser.Id,
				DiscordUsername = guildUser.Username,
				Points = 0,
			};

			db.Add(user);
		}

		user.Points = points;

		// TODO handle exceptions
		await db.SaveChangesAsync().ConfigureAwait(false);

		await UpdateRoles(discordUser.Id, user.Points).ConfigureAwait(false);

		await RespondAsync(
			$"Updated points for '{Context.User.GlobalName}' successfully ({user.Points} points)."
		).ConfigureAwait(false);
	}

	[SlashCommand("add-points", "Add/subtract points for a user")]
	[RequireRole(roleId: DiscordConstants.DiscordModsRoleId, Group = "Permission")]
	private async Task AddPoints(IUser discordUser, long amount)
	{
		using var db = new ApplicationDbContext();

		var user = await db.DiscordUsers.FirstOrDefaultAsync(u => u.DiscordId == discordUser.Id).ConfigureAwait(false);

		if (user == null)
		{
			var guildUser = Context.Guild.GetUser(discordUser.Id);
			if (guildUser == null)
			{
				await RespondAsync($"Invalid value for parameter {nameof(discordUser)}").ConfigureAwait(false);
				return;
			}

			user = new DiscordUser
			{
				DiscordId = discordUser.Id,
				DiscordUsername = guildUser.Username,
				Points = 0,
			};

			db.Add(user);
		}

		user.Points += amount;

		// TODO handle exceptions
		await db.SaveChangesAsync().ConfigureAwait(false);

		await UpdateRoles(discordUser.Id, user.Points).ConfigureAwait(false);

		await RespondAsync(
			$"Updated points for '{Context.User.GlobalName}' successfully ({user.Points} points)."
		).ConfigureAwait(false);
	}

	private async Task UpdateRoles(ulong userId, long currentPoints)
	{
		var guildUser = Context.Guild.GetUser(userId);

		// TODO read from env
		if (currentPoints >= 5000 && currentPoints < 20000)
		{
			await guildUser
				.AddRoleAsync(DiscordConstants.RegularRoleId)
				.ConfigureAwait(false);
		}
		else if (currentPoints == 20000 && currentPoints < 80000)
		{
			await guildUser
				.AddRoleAsync(DiscordConstants.DankRoleId)
				.ConfigureAwait(false);
		}
		else if (currentPoints >= 80000)
		{
			await guildUser
				.AddRoleAsync(DiscordConstants.BasedRoleId)
				.ConfigureAwait(false);
		}
		else
		{
			await guildUser.RemoveRolesAsync(
				new ulong[] {
					DiscordConstants.RegularRoleId,
					DiscordConstants.DankRoleId,
					DiscordConstants.BasedRoleId
				}
			).ConfigureAwait(false);
		}
	}
}
