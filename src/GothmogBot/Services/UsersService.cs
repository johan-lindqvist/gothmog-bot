using System.Collections.Immutable;
using Discord;
using Discord.Rest;
using GothmogBot.Database;
using GothmogBot.Discord;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Serilog;

namespace GothmogBot.Services;

public class UsersService
{
	private readonly DiscordRestClient restClient;
	private readonly IOptions<DiscordOptions> discordOptions;

	public UsersService(
		DiscordRestClient restClient,
		IOptions<DiscordOptions> discordOptions)
	{
		this.restClient = restClient;
		this.discordOptions = discordOptions;
	}

	public async Task<ImmutableList<RestGuildUser>> GetUsersAsync()
	{
		await restClient.LoginAsync(TokenType.Bot, discordOptions.Value.DiscordApiToken).ConfigureAwait(false);
		var guild = await restClient.GetGuildAsync(DiscordConstants.BulldogsKappaClubDiscordGuildId, RequestOptions.Default).ConfigureAwait(false);

		var users = ImmutableList.CreateBuilder<RestGuildUser>();
		await foreach (var userCollection in guild.GetUsersAsync().ConfigureAwait(false))
		{
			foreach (var user in userCollection)
			{
				if (!user.RoleIds.Any(roleId => DiscordConstants.AllRoles.Contains(roleId)))
				{
					continue;
				}

				Log.Information("Found MegaCuck {Name}, {UserId}", user.DisplayName, user.Id);
				users.Add(user);
			}
		}

		return users.ToImmutable();
	}
}
