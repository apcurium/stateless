namespace Amp.Logging
{
    // Empty class to ensure assembly is loaded (Fixes problem on iOS)
    public class LoggerBootstrap
    {
        public LoggerBootstrap()
        {
            var empty = AmpLoggingModule.LogFilePath;
        }
    }
}