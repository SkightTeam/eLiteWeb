using System;
using System.Collections.Generic;
using FluentBuild.MessageLoggers;
using FluentBuild.MessageLoggers.ConsoleMessageLoggers;
using FluentBuild.MessageLoggers.TeamCityMessageLoggers;
using FluentBuild.Utilities;

namespace FluentBuild
{
    ///<summary>
    /// Defaults for the fluent build runner
    ///</summary>
    public static class Defaults
    {
        ///<summary>
        /// Sets the behavior of what to do when an error occurs. The default is to fail.
        ///</summary>
        public static OnError OnError = OnError.Fail;

        ///<summary>
        /// Sets the .NET Framework version to use. The default is the highest desktop framework found.
        ///</summary>
        public static IFrameworkVersion FrameworkVersion;

        private static IMessageLogger _logger;

        static Defaults ()
		{
			//use the simple logger if on unix as the color setting relies on p/invoke
			if (IsRunningOnMono ()) {
				_logger = new MessageLoggerProxy (new SimpleMessageLogger ());
				FrameworkVersion=Utilities.FrameworkVersion.NET4_5;
			}
            else
            {
                _logger = new MessageLoggerProxy(new ConsoleMessageLogger());
                var frameworkVersionsToCheck = new List<IFrameworkVersion>();
                frameworkVersionsToCheck.Add(Utilities.FrameworkVersion.NET4_5);
                frameworkVersionsToCheck.Add(Utilities.FrameworkVersion.NET4_0.Full);
                frameworkVersionsToCheck.Add(Utilities.FrameworkVersion.NET4_0.Client);
                frameworkVersionsToCheck.Add(Utilities.FrameworkVersion.NET3_5);
                frameworkVersionsToCheck.Add(Utilities.FrameworkVersion.NET3_0);
                frameworkVersionsToCheck.Add(Utilities.FrameworkVersion.NET2_0);

                foreach (IFrameworkVersion frameworkVersion in frameworkVersionsToCheck)
                {
                    if (frameworkVersion.IsFrameworkInstalled())
                    {
                        FrameworkVersion = frameworkVersion;
                        return;
                    }
                }
            }
        }

        public static IMessageLogger Logger
        {
            get { return _logger; }
        }

        private static bool IsRunningOnMono()
        {
            //http://www.mono-project.com/FAQ:_Technical
            //var p = (int) Environment.OSVersion.Platform;
            //if ((p == 4) || (p == 6) || (p == 128)) //runnin
//            {
//                return true;
//            }
//
//            return false;
            Type t = Type.GetType("Mono.Runtime");
            if (t != null)
                return true;
            else
                return false;
        }

        public static void SetLogger(string logger)
        {
            switch (logger.ToUpper())
            {
                case "SIMPLE":
                    _logger = new MessageLoggerProxy(new SimpleMessageLogger());
                    break;
                case "CONSOLE":
                    _logger = new MessageLoggerProxy(new ConsoleMessageLogger());
                    break;
                case "TEAMCITY":
                    _logger = new MessageLoggerProxy(new MessageLogger());
                    break;
                default:
                    throw new ArgumentException("logger type " + logger + " unkown.");
            }
        }

        public static void SetLogger(IMessageLogger logger)
        {
            _logger = new MessageLoggerProxy(logger);
        }
    }
}