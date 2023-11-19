using GothmogBot.Services;
using Quartz;
using StratzClient;

namespace GothmogBot.Jobs;

public sealed class FetchDiscordUsersJob : IJob
{
	private readonly UsersService usersService;

	public FetchDiscordUsersJob(UsersService usersService)
	{
		this.usersService = usersService;
	}

	public async Task Execute(IJobExecutionContext context)
	{
		var users = await usersService.GetUsersAsync().ConfigureAwait(false);

		context.Result = users;
	}
}
