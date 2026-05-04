using is_payment_system.Model;
using is_payment_system.Service;
using Microsoft.EntityFrameworkCore;

namespace is_payment_system.Repository;

public class LogRepository
{
    public LogRepository()
    {
    }

    public List<LogEntry> Logs
    {
        get
        {
            using var ctx = new PaymentSystemDbContext();
            return (from l in ctx.Logs
                orderby l.Timestamp descending, l.Id descending
                select l).ToList();
        }
    }

    public List<LogEntry> FindByEventId(string eventId)
    {
        using var ctx = new PaymentSystemDbContext();
        return (from l in ctx.Logs
            where l.EventId == eventId
            orderby l.Timestamp descending, l.Id descending
            select l).ToList();
    }

    public List<LogEntry> FindByLoggerType(string loggerType)
    {
        using var ctx = new PaymentSystemDbContext();
        return (from l in ctx.Logs
            where l.LoggerType == loggerType
            orderby l.Timestamp descending, l.Id descending
            select l).ToList();
    }

    public int DeleteOldLogs(DateTime beforeDate)
    {
        using var ctx = new PaymentSystemDbContext();
        var oldLogs = (from l in ctx.Logs
            where l.Timestamp < beforeDate
            select l).ToList();

        if (oldLogs.Count == 0)
        {
            return 0;
        }

        ctx.Logs.RemoveRange(oldLogs);
        ctx.SaveChanges();
        return oldLogs.Count;
    }
}