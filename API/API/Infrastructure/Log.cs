namespace API.Infrastructure;

public interface ILog
{
    void Error(string message);
    void Info(string message);
}

public class Log : ILog
{
    public void Error(string message)
    {
        Console.WriteLine(message);
    }

    public void Info(string message)
    {
        Console.WriteLine(message);
    }
}