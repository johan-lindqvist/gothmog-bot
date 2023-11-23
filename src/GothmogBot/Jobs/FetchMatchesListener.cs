using Quartz;
using Quartz.Listener;
using StratzClient;

namespace GothmogBot.Jobs;

public class FetchMatchesListener : JobListenerSupport
{
	public override string Name => "FetchMatchesListener";

	private readonly Action<IGetPlayerMatchesResult> f;

	public FetchMatchesListener(Action<IGetPlayerMatchesResult> f)
	{
		this.f = f;
	}

	public override Task JobWasExecuted(
		IJobExecutionContext context,
		JobExecutionException? jobException,
		CancellationToken cancellationToken = default
	)
	{
		var matches = (IGetPlayerMatchesResult)context.Result;

		f(matches);

		return base.JobWasExecuted(context, jobException, cancellationToken);
	}
}
