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

    public static DateTime NowDate => new DateTime(Now.Year, Now.Month, Now.Day);

    private static DateTime initTime = DateTime.UtcNow;
    private static readonly Stopwatch sw = new();

    static DateTimeProvider()
    {
        sw.Start();
    }
}