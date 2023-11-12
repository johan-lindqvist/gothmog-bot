using System.Collections.Immutable;

namespace GothmogBot.Discord;

public static class RoleConstants
{
	public const ulong YouTubeTier2RoleId = 777437173904703540;
	public const ulong TwitchTier3SubscriberRoleId = 707480013406076951;
	public const ulong CustomMegaCuckRoleId = 948486251315609610;

	public static ImmutableList<ulong> AllRoles { get; } = ImmutableList.Create<ulong>(YouTubeTier2RoleId, TwitchTier3SubscriberRoleId, CustomMegaCuckRoleId);
}
