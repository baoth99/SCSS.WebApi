using System;


namespace SCSS.AWSService.Interfaces
{
    public interface ILoggerService
    {
        void LogInfo(string message);

        void LogWarn(string message);

        void LogDebug(string message);

        void LogError(Exception ex, string message);
    }
}
