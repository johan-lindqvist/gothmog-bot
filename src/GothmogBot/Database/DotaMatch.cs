using System.Collections.ObjectModel;

namespace GothmogBot.Database;

public sealed record DotaMatch
{
	public long Id { get; set; }

	public DateTime StartDateTime { get; set; }

	public DateTime EndDateTime { get; set; }

	public KeyedCollection<long, User> Users { get; set; }
}
