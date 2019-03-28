namespace Settlers.Toolbox.Infrastructure.Logging.Logger.Interfaces
{
    public interface IInternalLogger
    {
        void Log(LogLevel logLevel, string message);
    }
}