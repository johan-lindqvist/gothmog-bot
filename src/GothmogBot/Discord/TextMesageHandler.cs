using Discord.Commands;
using Discord.WebSocket;
using GothmogBot.Database;
using Microsoft.EntityFrameworkCore;

namespace GothmogBot.Discord;

public class TextMessageHandler
{
    private readonly DiscordSocketClient client;

    public TextMessageHandler(DiscordSocketClient client)
    {
        this.client = client;
    }

    public async Task InitializeAsync()
    {
        client.MessageReceived += HandleMessageAsync;
    }

    public async Task HandleMessageAsync(SocketMessage messageParam)
    {
        if (messageParam is not SocketUserMessage message) return;

        var id = message.Author.Id;

        using var db = new ApplicationDbContext();

        var discordUser = await db.DiscordUsers.FirstOrDefaultAsync(u => u.DiscordId == id).ConfigureAwait(false);

        if (discordUser == null)
        {
            discordUser = new DiscordUser
            {
                DiscordUsername = message.Author.Username,
                DiscordId = message.Author.Id
            };

            await db.AddAsync(discordUser).ConfigureAwait(false);
        }

        if (discordUser.HourlyPoints >= DiscordConstants.MaxPointsPerHour
            || discordUser.DailyPoints >= DiscordConstants.MaxPointsPerDay
            || discordUser.WeeklyPoints >= DiscordConstants.MaxPointsPerWeek)
        {
            return;
        }

        discordUser.HourlyPoints += DiscordConstants.PointsPerMessage;
        discordUser.DailyPoints += DiscordConstants.PointsPerMessage;
        discordUser.WeeklyPoints += DiscordConstants.PointsPerMessage;
        discordUser.Points += DiscordConstants.PointsPerMessage;

        await db.SaveChangesAsync().ConfigureAwait(false);

        var guildUser = client
                .GetGuild(DiscordConstants.BulldogsKappaClubDiscordGuildId)
                .GetUser(message.Author.Id);

        // TODO read from env
        if (discordUser.Points >= 5000 && discordUser.Points < 20000)
        {
            await guildUser
                .AddRoleAsync(DiscordConstants.RegularRoleId)
                .ConfigureAwait(false);
        }
        else if (discordUser.Points == 20000 && discordUser.Points < 80000)
        {
            await guildUser
                .AddRoleAsync(DiscordConstants.DankRoleId)
                .ConfigureAwait(false);
        }
        else if (discordUser.Points >= 80000)
        {
            await guildUser
                .AddRoleAsync(DiscordConstants.BasedRoleId)
                .ConfigureAwait(false);
        }
        else
        {
            await guildUser.RemoveRolesAsync(
                new ulong[] {
                    DiscordConstants.RegularRoleId,
                    DiscordConstants.DankRoleId,
                    DiscordConstants.BasedRoleId
                }
            ).ConfigureAwait(false);
        }
    }
}