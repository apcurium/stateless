using System;
using Amp.Logging;

namespace TelephoneCallExample
{
    public class Logger : ILogger
    {
        private readonly log4net.ILog _logger;

        public Logger(string name)
        {
            _logger = log4net.LogManager.GetLogger(name);
        }

        public string GetLogFilePath()
        {
            return string.Empty;
        }

        public void Log(LogLevel level, string message, params object[] args)
        {
            switch (level)
            {
                case LogLevel.Debug:
                    _logger.Debug(message);
                    break;
                case LogLevel.Info:
                    _logger.Info(message);
                    break;
                case LogLevel.Warn:
                    _logger.Warn(message);
                    break;
                case LogLevel.Error:
                    _logger.Error(message);
                    break;
                case LogLevel.Fatal:
                    _logger.Fatal(message);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public void Log(LogLevel level, Exception e, string message, params object[] args)
        {
            switch (level)
            {
                case LogLevel.Debug:
                    _logger.Debug(message, e);
                    break;
                case LogLevel.Info:
                    _logger.Info(message, e);
                    break;
                case LogLevel.Warn:
                    _logger.Warn(message, e);
                    break;
                case LogLevel.Error:
                    _logger.Error(message, e);
                    break;
                case LogLevel.Fatal:
                    _logger.Fatal(message, e);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}

