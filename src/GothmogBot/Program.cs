using System.Globalization;
using Discord;
using Discord.Interactions;
using Discord.Rest;
using Discord.WebSocket;
using GothmogBot;
using GothmogBot.Discord;
using GothmogBot.Pairing;
using GothmogBot.Pairing.OAuth;
using GothmogBot.Services;
using GothmogBot.Stratz;
using Microsoft.AspNetCore.Mvc;
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

builder.Services
	.AddOptions<StratzOptions>()
	.Bind(configuration.GetSection(StratzOptions.SectionName))
	.Validate(o => !string.IsNullOrEmpty(o.StratzApiToken), "StratzApiToken must have a value.");

builder.Services
	.AddOptions<DiscordOAuthOptions>()
	.Bind(configuration.GetSection(DiscordOAuthOptions.SectionName))
	.Validate(o => !string.IsNullOrEmpty(o.ClientId), "ClientId must have a value.")
	.Validate(o => !string.IsNullOrEmpty(o.ClientSecret), "ClientSecret must have a value.")
	.Validate(o =>  !string.IsNullOrEmpty(o.RedirectUrl), "RedirectUrl must have a value.");

var stratzOptions = configuration
	.GetSection(StratzOptions.SectionName)
	.Get<StratzOptions>() ?? throw new InvalidOperationException("No StratzOptions provided.");

var discordOAuthOptions = configuration
	.GetSection(DiscordOAuthOptions.SectionName)
	.Get<DiscordOAuthOptions>() ?? throw new InvalidOperationException("No DiscordOAuth provided.");
;

// Add HttpClient
builder.Services.AddHttpClient();

// Add Stratz GraphQL Client
builder.Services.AddStratzGraphQLClient(stratzOptions.StratzApiToken);

// Add serilog
builder.Host.UseSerilog();
builder.Logging.AddSerilog();

var loggerConfiguration = new LoggerConfiguration()
	.WriteTo.Console(formatProvider: CultureInfo.CurrentCulture)
	.Enrich.FromLogContext();

Log.Logger = loggerConfiguration.CreateLogger();

// Add Discord Services
#pragma warning disable CA2000
builder.Services.AddSingleton(new DiscordSocketClient(new DiscordSocketConfig { GatewayIntents = GatewayIntents.None }));
#pragma warning restore CA2000
builder.Services.AddTransient<DiscordRestClient>();

// Add Discord OAuth

// Add local services
builder.Services.AddSingleton<IDiscordClientRunner, DiscordClientRunner>();
builder.Services.AddSingleton<InteractionService>(services => new InteractionService(services.GetRequiredService<DiscordSocketClient>()));
builder.Services.AddSingleton<InteractionHandler>();
builder.Services.AddSingleton<DotaService>();
builder.Services.AddSingleton<DiscordOAuthService>();
builder.Services.AddSingleton<PairingService>();

// Build and run app
var app = builder.Build();

app.MapGet("/pair", () => Results.Redirect(DiscordOAuthUrlHelper.GenerateRedirectUrl(discordOAuthOptions.ClientId, discordOAuthOptions.RedirectUrl).ToString()));

app.MapGet("/pair-callback", async ([FromServices] PairingService pairingService, HttpContext context) =>
{
	var code = context.Request.Query["code"].FirstOrDefault();

	if (string.IsNullOrWhiteSpace(code))
	{
		Log.Warning("Missing code in callback");
		return Results.BadRequest();
	}

	// TODO: handle if failed result?
	var pairResult = await pairingService.PairUserAsync(code).ConfigureAwait(false);

	// TODO: return html telling the user the pairing succeeded? or message them on discord maybe?
	return Results.Ok();
});

var discordRunner = app.Services.GetRequiredService<IDiscordClientRunner>();

var appRunTask = app.RunAsync();
var discordRunnerTask = discordRunner.RunAsync();

await Task.WhenAny(discordRunnerTask, appRunTask).ConfigureAwait(false);
