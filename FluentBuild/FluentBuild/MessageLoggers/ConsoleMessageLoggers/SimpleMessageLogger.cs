using System;

namespace FluentBuild.MessageLoggers.ConsoleMessageLoggers
{
    internal class SimpleMessageLogger:IMessageLogger
    {
        public VerbosityLevel Verbosity
        {
            get { throw new NotImplementedException("implemented by proxy"); }
            set { throw new NotImplementedException("implemented by proxy"); }
        }

        public void WriteHeader(string header)
        {
            Console.WriteLine(header);
        }

        public void WriteDebugMessage(string message)
        {
            Write("DEBUG", message);
        }

        public void Write(string type, string message, params string[] items)
        {
            var data = string.Format(message, items);
            string outputMessage = String.Format("  [{0}] {1}", type, data);
            Console.WriteLine(outputMessage);
        }

        public void WriteError(string type, string message)
        {
            Write(type, message);
        }

        public void WriteWarning(string type, string message)
        {
            Write(type, message);
        }

        public IDisposable ShowDebugMessages
        {
            get { throw new NotImplementedException("This should only be handled from a proxy wrapping this logger"); }
        }

        public ITestSuiteMessageLogger WriteTestSuiteStarted(string name)
        {
            return new TestSuiteLogger(0, name);
        }
    }
}