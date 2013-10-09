using System;

namespace FluentBuild.MessageLoggers
{
    public class MessageLoggerProxy : IMessageLogger
    {
        internal IMessageLogger InternalLogger;

        private static VerbosityLevel _verbosity;
        public VerbosityLevel Verbosity
        {
            get { return _verbosity; }
            set { _verbosity = value; }
        }

        public MessageLoggerProxy(IMessageLogger internalLogger)
        {
            InternalLogger = internalLogger;
        }

        public void WriteHeader(string header)
        {
            if (Verbosity >= VerbosityLevel.TaskNamesOnly)
            {
                InternalLogger.WriteHeader(header);
            }
        }

        public void WriteDebugMessage(string message)
        {
            if (Verbosity >= VerbosityLevel.Full)
                InternalLogger.WriteDebugMessage(message);
        }

        public void Write(string type, string message, params string[] items)
        {
            InternalLogger.Write(type,message, items);
        }

        public void Write(string type, string message, string statusDescription)
        {
            InternalLogger.Write(type, message, statusDescription);
        }

        public void WriteError(string type, string message)
        {
            InternalLogger.WriteError(type, message);
        }

        public void WriteWarning(string type, string message)
        {
            InternalLogger.WriteWarning(type, message);
        }

        public IDisposable ShowDebugMessages
        {
            get { return new DebugMessages(this); }
        }

        public ITestSuiteMessageLogger WriteTestSuiteStarted(string name)
        {
            return InternalLogger.WriteTestSuiteStarted(name);
        }
    }
}