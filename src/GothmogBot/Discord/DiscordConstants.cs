using System.Collections.Immutable;

namespace GothmogBot.Discord;

public static class DiscordConstants
{
	public const ulong BulldogsKappaClubDiscordGuildId = 111772771016515584;
	public const ulong YouTubeTier2RoleId = 777437173904703540;
	public const ulong TwitchTier3SubscriberRoleId = 707480013406076951;
	public const ulong CustomMegaCuckRoleId = 948486251315609610;
	public const ulong DiscordModsRoleId = 117296318052958214;
	public const ulong RegularRoleId = 345877423142731778;
	public const ulong DankRoleId = 353238417212964865;
	public const ulong BasedRoleId = 814422920193245214;

	public const long PointsPerMessage = 10;
	public const long MaxPointsPerHour = 30;
	public const long MaxPointsPerDay = 120;
	public const long MaxPointsPerWeek = 480;


	public static ImmutableList<ulong> AllRoles { get; } = ImmutableList.Create(YouTubeTier2RoleId, TwitchTier3SubscriberRoleId, CustomMegaCuckRoleId);
}
