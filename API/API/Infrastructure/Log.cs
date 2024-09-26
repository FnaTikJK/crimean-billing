namespace API.Infrastructure;

public interface ILog
{
    void Error(string message);
}

public class Log : ILog
{
    public void Error(string message)
    {
        throw new NotImplementedException();
    }
}