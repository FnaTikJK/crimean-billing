using System.Diagnostics;

namespace API.Infrastructure;

public static class DateTimeProvider
{
    public static DateTime Now
    {
        get => initTime + sw.Elapsed;
        set
        {
            initTime = value;
            sw.Restart();
        }
    }

    private static DateTime initTime = DateTime.UtcNow;
    private static readonly Stopwatch sw = new();
}