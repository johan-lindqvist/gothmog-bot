
using Microsoft.EntityFrameworkCore;

namespace GothmogBot.Database;

[PrimaryKey(nameof(SteamAccountId))]
public sealed record User
{
	public long SteamAccountId { get; set; }

	public string TwitchUsername { get; set; }

	public string DiscordUsername { get; set; }

	public List<DotaMatch> DotaMatches { get; set; }
}
