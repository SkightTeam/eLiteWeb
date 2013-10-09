using System;


namespace FluentBuild.MessageLoggers.ConsoleMessageLoggers
{
    internal class TestLogger : ITestLogger
    {
        private readonly string _testName;
        private int _indentation;


        public TestLogger(int indentation, string testName)
        {
            _indentation = indentation;
            _testName = testName.Substring(testName.LastIndexOf(".") +1); //strip out the suite name from the test name
        }

        public void WriteTestPassed(TimeSpan duration)
        {
            Utilities.ConsoleColor.SetColor(Utilities.ConsoleColor.BuildColor.BrightGreen);
            WriteMessage(_testName, "Passed " + duration.TotalSeconds.ToString("N3") + "s");
            Utilities.ConsoleColor.SetColor(Utilities.ConsoleColor.BuildColor.White);
        }

        internal void WriteMessage(string name, string data)
        {
            var remainingWidth = ConsoleMessageLogger.WindowWidth-11 - _indentation- name.Length - data.Length;
            if (remainingWidth <=0)
            {
                remainingWidth = 0;
            }
            Defaults.Logger.Write("TEST", "".PadRight(_indentation, ' ') + name + "".PadRight(remainingWidth, '.') + " " + data);
        }

        public void WriteTestIgnored(string message)
        {
            Utilities.ConsoleColor.SetColor(Utilities.ConsoleColor.BuildColor.BrightYellow);
            WriteMessage(_testName, "Ignored");
            Utilities.ConsoleColor.SetColor(Utilities.ConsoleColor.BuildColor.White);
        }

        public void WriteTestFailed(string message, string details)
        {
            Utilities.ConsoleColor.SetColor(Utilities.ConsoleColor.BuildColor.BrightRed);
            WriteMessage(_testName, "Failed");
            Defaults.Logger.Write("Details", message);
            Defaults.Logger.Write("StackTrace", details);
            Utilities.ConsoleColor.SetColor(Utilities.ConsoleColor.BuildColor.White);
        }
    }
}