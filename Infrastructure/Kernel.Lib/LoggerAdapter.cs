namespace Kernel.Lib;

public class LoggerAdapter<T> : ILoggerAdapter<T>
{
    private readonly ILogger _logger;

    public LoggerAdapter(ILogger<T> logger)
    {
        _logger = logger;
    }

    public void LogInformation(Exception exception, string message, params object[] args)
    {
        if (_logger.IsEnabled(LogLevel.Information))
        {
            _logger.LogInformation(exception, message, args);
        }
    }

    public void LogInformation(string message, params object[] args)
    {
        if (_logger.IsEnabled(LogLevel.Information))
        {
            _logger.LogInformation(message, args);
        }
    }

    public void LogDebug(string message, params object[] args)
    {
        if (_logger.IsEnabled(LogLevel.Debug))
        {
            _logger.LogDebug(message, args);
        }
    }

    public void LogDebug(Exception exception, string message, params object[] args)
    {
        if (_logger.IsEnabled(LogLevel.Debug))
        {
            _logger.LogDebug(exception, message, args);
        }
    }
    public void LogWarning(string message, params object[] args)
    {
        if (_logger.IsEnabled(LogLevel.Warning))
        {
            _logger.LogWarning(message, args);
        }
    }

    public void LogWarning(Exception exception, string message, params object[] args)
    {
        if (_logger.IsEnabled(LogLevel.Warning))
        {
            _logger.LogWarning(exception, message, args);
        }
    }

    public void LogError(string message, params object[] args)
    {
        if (_logger.IsEnabled(LogLevel.Error))
        {
            _logger.LogError(message, args);
        }
    }

    public void LogError(Exception exception, string message, params object[] args)
    {
        if (_logger.IsEnabled(LogLevel.Error))
        {
            _logger.LogError(exception, message, args);
        }
    }

    public void LogTrace(string message, params object[] args)
    {
        if (_logger.IsEnabled(LogLevel.Trace))
        {
            _logger.LogTrace(message, args);
        }
    }

    public void LogTrace(Exception exception, string message, params object[] args)
    {
        if (_logger.IsEnabled(LogLevel.Trace))
        {
            _logger.LogTrace(exception, message, args);
        }
    }
}