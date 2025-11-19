namespace Kernel.Lib.Interface;

public interface ILoggerAdapter<T>
{
    void LogInformation(Exception exception, string message, params object[] args);
    void LogInformation(string message, params object[] args);
    void LogDebug(string message, params object[] args);
    void LogDebug(Exception exception, string message, params object[] args);
    void LogError(string message, params object[] args);
    void LogError(Exception exception, string message, params object[] args);
    void LogTrace(string message, params object[] args);
    void LogTrace(Exception exception, string message, params object[] args);
}