using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace ProjectLambdas.Models;

public class ConsoleLogger : Microsoft.Extensions.Logging.ILogger
{
    public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
    {
        throw new NotImplementedException();
    }

    public bool IsEnabled(LogLevel logLevel) => true;
    
    public IDisposable? BeginScope<TState>(TState state) where TState : notnull => null!;
}
