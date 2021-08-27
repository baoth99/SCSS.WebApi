using NLog;
using SCSS.AWSService.Interfaces;
using System;

namespace SCSS.AWSService.Implementations
{
    public class LoggerService : ILoggerService
    {
        private static ILogger _logger ;

        public LoggerService()
        {
            _logger = LogManager.GetCurrentClassLogger();
        }

        public void LogDebug(string message)
        {
            _logger.Debug(message);
        }

        public void LogError(Exception ex, string message)
        {
            _logger.Error(ex, message);
        }

        public void LogInfo(string message)
        {
            _logger.Info(message);
        }

        public void LogWarn(string message)
        {
            _logger.Warn(message);
        }
    }
}
