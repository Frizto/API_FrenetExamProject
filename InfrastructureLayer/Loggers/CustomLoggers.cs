using NLog;
namespace InfrastructureLayer.Loggers;

public static class CustomLoggers
{
    public static readonly Logger CreateLogger = LogManager.GetLogger("CreateLogger");
    public static readonly Logger ErrorLogger = LogManager.GetLogger("ErrorLogger");
    public static readonly Logger UpdateLogger = LogManager.GetLogger("UpdateLogger");
    public static readonly Logger DeleteLogger = LogManager.GetLogger("DeleteLogger");
}