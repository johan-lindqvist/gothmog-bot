using Discord;
using Discord.Net;
using Discord.Rest;
using Discord.WebSocket;
using Serilog;

namespace GothmogBot.Discord.Commands;

public class SyncUsersCommand
{
	private readonly DiscordSocketClient socketClient;
	private readonly DiscordRestClient restClient;

	public SyncUsersCommand(
		IDiscordClientRunner discordClientRunner)
	{
		socketClient = discordClientRunner.DiscordSocketClient;
		restClient = discordClientRunner.DiscordRestClient;

		socketClient.Ready += HandleDiscordClientReadyAsync;
		socketClient.SlashCommandExecuted += SlashCommandHandlerAsync;
	}

	private async Task HandleDiscordClientReadyAsync()
	{
		var guild = await restClient.GetGuildAsync(DiscordConstants.BulldogsKappaClubDiscordGuildId);

		var guildCommand = new SlashCommandBuilder
		{
			Name = "sync-users-command",
			Description = "Syncs MC users"
		};

		try
		{
			await guild.CreateApplicationCommandAsync(guildCommand.Build());

			Log.Information("Sync users command created");
		}
		catch (HttpException e)
		{
			Log.Error("Error when creating command {CommandName}. Errors: {Errors}", guildCommand.Name, string.Join(", ", e.Errors));
		}
	}

	private async Task SlashCommandHandlerAsync(SocketSlashCommand command)
	{
		await command.RespondAsync($"Command {command.Data.Name} executed, wow");
	}
}
