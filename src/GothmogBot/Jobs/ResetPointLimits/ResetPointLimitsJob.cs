using GothmogBot.Database;
using Microsoft.EntityFrameworkCore;
using Quartz;
using Serilog;

namespace GothmogBot.Jobs.ResetPointLimits;

public sealed class ResetPointLimitsJob : IJob
{
    public enum Type
    {
        Hourly,
        Daily,
        Weekly
    }

    public async Task Execute(IJobExecutionContext context)
    {
        var type = context?.MergedJobDataMap["ResetPointLimitsType"] as Type?;

        Log.Write(Serilog.Events.LogEventLevel.Information, "Resetting points [{type}]", type);

        if (type == null)
        {
            // TODO log error
            return;
        }

        using var db = new ApplicationDbContext();

        await db.DiscordUsers.ForEachAsync((u) =>
        {
            u.HourlyPoints = 0;

            if (type == Type.Daily || type == Type.Weekly)
            {
                u.DailyPoints = 0;
            }

            if (type == Type.Weekly)
            {
                u.WeeklyPoints = 0;
            }
        }).ConfigureAwait(false);

        // TODO handle exceptions
        await db.SaveChangesAsync().ConfigureAwait(false);
    }

}