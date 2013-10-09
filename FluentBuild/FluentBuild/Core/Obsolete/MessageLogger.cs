using System;
using FluentBuild.MessageLoggers;
using FluentBuild.MessageLoggers.ConsoleMessageLoggers;
using FluentBuild.MessageLoggers.TeamCityMessageLoggers;
using FluentBuild.Utilities;

namespace FluentBuild.Core
{
    [Obsolete("Replaced by Defaults.Logger factory", true)]
    public class MessageLogger
    {
        internal static IMessageLogger InternalLogger;

        static MessageLogger()
        {
            InternalLogger = new ConsoleMessageLogger();
        }

        ///<summary>
        /// Gets or sets the message logging verbosity level
        ///</summary>
        public static VerbosityLevel Verbosity { get; set; }

        

        public static IDisposable ShowDebugMessages
        {
            get { return new DebugMessages(null); }
        }

        public static ITestSuiteMessageLogger WriteTestSuiteStarted(string name)
        {
            return InternalLogger.WriteTestSuiteStarted(name);
        }

        public static void WriteHeader(string header)
        {
            if (Verbosity >= VerbosityLevel.TaskNamesOnly)
            {
                InternalLogger.WriteHeader(header);
            }
        }


        public static void WriteDebugMessage(string message)
        {
            if (Verbosity >= VerbosityLevel.Full)
                InternalLogger.WriteDebugMessage(message);
        }


        public static void Write(string type, string message)
        {
            InternalLogger.Write(type, message);
        }
        
        public static void WriteError(string message)
        {
            InternalLogger.WriteError("ERROR", message);
        }

        public static void WriteError(string prefix, string message)
        {
            InternalLogger.WriteError(prefix, message);
        }

        public static void WriteWarning(string prefix, string message)
        {
            InternalLogger.WriteWarning(prefix, message);
        }


        public static void SetLogger(IMessageLogger logger)
        {
            InternalLogger = logger;
        }

        public static void SetLogger(string logger)
        {
            switch (logger.ToUpper())
            {
                case "CONSOLE":
                    InternalLogger = new ConsoleMessageLogger();
                    break;
                case "TEAMCITY":
                    InternalLogger = new MessageLoggers.TeamCityMessageLoggers.MessageLogger();
                    break;
                default:
                    throw new ArgumentException("logger type " + logger + " unkown.");
            }
        }
    }
}