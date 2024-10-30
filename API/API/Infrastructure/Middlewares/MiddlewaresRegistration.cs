namespace API.Infrastructure.Middlewares;

public static class MiddlewaresRegistration
{
    public static void RegisterMiddlewares(this WebApplication app)
    {
        app.UseMiddleware<ExceptionHandlerMiddleware>();
    }
}