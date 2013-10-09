using System;


namespace FluentBuild.MessageLoggers.TeamCityMessageLoggers
{
    internal class MessageLogger : IMessageLogger
    {
        internal string _currentHeader;

        #region IMessageLogger Members

        public void WriteHeader(string header)
        {
            if (!String.IsNullOrEmpty(_currentHeader))
                Console.WriteLine(String.Format("##teamcity[blockClosed name='{0}']", _currentHeader));

            _currentHeader = header;
            Console.WriteLine(String.Format("##teamcity[blockOpened name='{0}']", header));
        }

        public void WriteDebugMessage(string message)
        {
            Write("DEBUG", message);
        }

        public void Write(string type, string message, params string[] items)
        {
            var data = string.Format(message, items);
            string outputMessage = String.Format("[{0}] {1}", type, data);
            WriteMessage(outputMessage);
        }

        public void WriteError(string type, string message)
        {
            WriteMessage(message,message, "ERROR");
        }

        public void WriteWarning(string type, string message)
        {
            var outputMessage = String.Format("[{0}] {1}", type, message);
            WriteMessage(outputMessage, string.Empty, "WARNING");
        }

        public IDisposable ShowDebugMessages
        {
            get { throw new NotImplementedException("This should only be handled from a proxy wrapping this logger"); }
        }

        public ITestSuiteMessageLogger WriteTestSuiteStarted(string name)
        {
            return new TestSuiteMessageLogger(name);
        }

        public VerbosityLevel Verbosity
        {
            get { throw new NotImplementedException("implemented by proxy"); }
            set { throw new NotImplementedException("implemented by proxy"); }
        }

        #endregion

        private static void WriteMessage(string message)
        {
            WriteMessage(message, string.Empty, "NORMAL");
        }

        private static void WriteMessage(string message, string error, string type)
        {
            //NORMAL, WARNING, FAILURE, ERROR
            message = EscapeCharacters(message);
            error = EscapeCharacters(error);
            Console.WriteLine(String.Format("##teamcity[message text='{0}' errorDetails='{1}' status='{2}']", message,
                                            error, type));
        }

        internal static string EscapeCharacters(string data)
        {
            return data.Replace("|", "||").Replace("'", "|'").Replace("\n", "|n").Replace("\r", "|r").Replace("]", "|]");
        }
    }
}