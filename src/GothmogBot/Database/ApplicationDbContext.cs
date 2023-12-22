using Microsoft.EntityFrameworkCore;

namespace GothmogBot.Database;

public class ApplicationDbContext : DbContext
{
	private readonly string dbPath;

	public ApplicationDbContext()
	{
		var folder = Environment.SpecialFolder.LocalApplicationData;
		var path = Environment.GetFolderPath(folder);

		dbPath = Path.Join(path, "application.db");
	}

	public DbSet<User> Users { get; set; } = null!;

	public DbSet<DotaMatch> DotaMatches { get; set; } = null!;

	public DbSet<DiscordUser> DiscordUsers { get; set; } = null;

	protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) => optionsBuilder.UseSqlite($"Data Source={dbPath}");
}
