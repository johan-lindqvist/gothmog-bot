using System.Globalization;
using Discord.Rest;
using Discord.WebSocket;
using GothmogBot.Discord;
using GothmogBot.Discord.Commands;
using GothmogBot.Services;
using Serilog;

IConfiguration configuration = new ConfigurationBuilder()
	.SetBasePath(Environment.CurrentDirectory)
	.AddJsonFile("local.settings.json", optional: false, reloadOnChange: true)
	.AddEnvironmentVariables()
	.Build();

// Create builder
var builder = WebApplication.CreateBuilder(args);

// Add options
builder.Services
	.AddOptions<DiscordOptions>()
	.Bind(configuration.GetSection(DiscordOptions.SectionName))
	.Validate(o => !string.IsNullOrEmpty(o.DiscordApiToken), "DiscordApiToken must have a value.");

// Add HttpClient
builder.Services.AddHttpClient();

// Add serilog
builder.Host.UseSerilog();
builder.Logging.AddSerilog();

var loggerConfiguration = new LoggerConfiguration()
	.WriteTo.Console(formatProvider: CultureInfo.CurrentCulture)
	.Enrich.FromLogContext();

Log.Logger = loggerConfiguration.CreateLogger();

// Add local services
builder.Services.AddSingleton<IDiscordClientRunner, DiscordClientRunner>();
builder.Services.AddSingleton<UsersService>();
builder.Services.AddSingleton<SyncUsersCommand>();

// Add Discord Services
builder.Services.AddSingleton<DiscordSocketClient>();
builder.Services.AddSingleton<DiscordRestClient>();

// Build and run app
var app = builder.Build();

var discordRunner = app.Services.GetRequiredService<IDiscordClientRunner>();

var appRunTask = app.RunAsync();
var discordRunnerTask = discordRunner.RunAsync();

var usersService = app.Services.GetRequiredService<SyncUsersCommand>();

await Task.WhenAll(discordRunnerTask, appRunTask).ConfigureAwait(false);
