using System.Collections.Immutable;
using Discord;
using Discord.Rest;
using GothmogBot.Discord;
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

	public async Task GetUsersAsync()
	{
		await restClient.LoginAsync(TokenType.Bot, discordOptions.Value.DiscordApiToken);
		var guild = await restClient.GetGuildAsync(111772771016515584, RequestOptions.Default);

		var users = ImmutableList.CreateBuilder<RestGuildUser>();
		await foreach (var userCollection in guild.GetUsersAsync())
		{
			foreach (var user in userCollection)
			{
				if (!user.RoleIds.Any(roleId => RoleConstants.AllRoles.Contains(roleId)))
				{
					continue;
				}

				Log.Information("Found MegaCuck {Name}, {UserId}", user.DisplayName, user.Id);
				users.Add(user);
			}
		}
	}
}
