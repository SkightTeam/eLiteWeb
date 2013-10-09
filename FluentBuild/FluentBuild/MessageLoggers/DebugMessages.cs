using System;

namespace FluentBuild.MessageLoggers
{
    public class DebugMessages : IDisposable
    {
        private readonly MessageLoggerProxy _messageLogger;
        private readonly VerbosityLevel _originalVerbosity;
        public DebugMessages(MessageLoggerProxy messageLogger)
        {
            _messageLogger = messageLogger;
            _originalVerbosity = _messageLogger.Verbosity;
            _messageLogger.Verbosity = VerbosityLevel.Full;
        }

        public void Dispose()
        {
            _messageLogger.Verbosity = _originalVerbosity;
        }
    }
}