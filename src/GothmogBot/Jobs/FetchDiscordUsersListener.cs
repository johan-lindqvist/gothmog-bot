using Discord.Rest;
using Quartz;
using Quartz.Listener;
using StratzClient;
using System.Collections.Immutable;

namespace GothmogBot.Jobs;

public class FetchDiscordUsersListener : JobListenerSupport
{
	public override string Name => "FetchDiscordUsersListener";

	private readonly Action<ImmutableList<RestGuildUser>> f;

	public FetchDiscordUsersListener(Action<ImmutableList<RestGuildUser>> f)
	{
		this.f = f;
	}

	public override Task JobWasExecuted(
		IJobExecutionContext context,
		JobExecutionException? jobException,
		CancellationToken cancellationToken = default
	)
	{
		var users = (ImmutableList<RestGuildUser>)context.Result;

		f(users);

		return base.JobWasExecuted(context, jobException, cancellationToken);
	}
}
