using Quartz;
using StratzClient;

namespace GothmogBot.Jobs;

public sealed class FetchMatchesJob : IJob
{
	private readonly IStratzClient stratzClient;

	public FetchMatchesJob(IStratzClient stratzClient)
	{
		this.stratzClient = stratzClient;
	}

	public async Task Execute(IJobExecutionContext context)
	{
		var steamId = context.JobDetail.JobDataMap.GetLong("steamId");

		var matches = await stratzClient.GetPlayerMatches.ExecuteAsync(steamId).ConfigureAwait(false);

		context.Result = matches.Data;
	}
}
