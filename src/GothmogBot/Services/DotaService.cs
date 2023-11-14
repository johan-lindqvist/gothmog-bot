using GothmogBot.Stratz;
using StratzClient;

namespace GothmogBot.Services;

public sealed class DotaService
{
	private readonly IStratzClient stratzClient;

	public DotaService(IStratzClient stratzClient)
	{
		this.stratzClient = stratzClient;
	}

	public async Task GetDotaMatchAsync(CancellationToken ct)
	{
		var matches = await stratzClient.GetPlayerMatches.ExecuteAsync(StratzConstants.BulldogSteamId, ct);
	}
}
