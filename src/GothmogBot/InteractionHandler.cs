using System.Reflection;
using Discord;
using Discord.Interactions;
using Discord.WebSocket;
using GothmogBot.Discord;
using Serilog;

namespace GothmogBot;

public sealed class InteractionHandler
{
	private readonly DiscordSocketClient client;
	private readonly InteractionService handler;
	private readonly IServiceProvider services;

	public InteractionHandler(
		DiscordSocketClient client,
		InteractionService handler,
		IServiceProvider services)
	{
		this.client = client;
		this.handler = handler;
		this.services = services;
	}

	public async Task InitializeAsync()
	{
		client.Ready += ReadyAsync;
		handler.Log += DiscordLogger.LogAsync;

		// Add the public modules that inherit InteractionModuleBase<T> to the InteractionService
		await handler.AddModulesAsync(Assembly.GetEntryAssembly(), services).ConfigureAwait(false);

	// Process the InteractionCreated payloads to execute Interactions commands
		client.InteractionCreated += HandleInteraction;

		Log.Information("Interaction handler initialized");
	}

	private async Task ReadyAsync()
	{
		var removeTask = Task.Run(async () => await handler.RemoveModulesFromGuildAsync(DiscordConstants.BulldogsKappaClubDiscordGuildId).ConfigureAwait(false));
		var registerTask = Task.Run(async () => await handler.RegisterCommandsToGuildAsync(DiscordConstants.BulldogsKappaClubDiscordGuildId).ConfigureAwait(false));

		await Task.WhenAll(removeTask, registerTask).ConfigureAwait(false);
	}

	private async Task HandleInteraction(SocketInteraction interaction)
	{
		try
		{
			// Create an execution context that matches the generic type parameter of your InteractionModuleBase<T> modules.
			var context = new SocketInteractionContext(client, interaction);

			// Execute the incoming command.
			var result = await handler.ExecuteCommandAsync(context, services).ConfigureAwait(false);

			if (!result.IsSuccess)
				switch (result.Error)
				{
					case InteractionCommandError.UnmetPrecondition:
						// TODO: implement
						break;
					default:
						break;
				}
		}
		catch (InvalidOperationException)
		{
			// If Slash Command execution fails it is most likely that the original interaction acknowledgement will persist. It is a good idea to delete the original
			// response, or at least let the user know that something went wrong during the command execution.
			if (interaction.Type is InteractionType.ApplicationCommand)
#pragma warning disable CA2008
#pragma warning disable CA1849
				await interaction.GetOriginalResponseAsync().ContinueWith(async (msg) => await msg.Result.DeleteAsync().ConfigureAwait(false)).ConfigureAwait(false);
#pragma warning restore CA1849
#pragma warning restore CA2008
		}
	}
}
