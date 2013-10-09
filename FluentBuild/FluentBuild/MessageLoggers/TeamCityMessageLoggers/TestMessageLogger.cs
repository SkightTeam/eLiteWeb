using System;

namespace FluentBuild.MessageLoggers.TeamCityMessageLoggers
{
    internal class TestMessageLogger : ITestLogger
    {
        private readonly string _testName;

        public TestMessageLogger(string testName)
        {
            _testName = testName;
        }

        private void WriteTestFinished(double duration)
        {
            Console.WriteLine(String.Format("##teamcity[testFinished name='{0}' duration='{1}']", MessageLogger.EscapeCharacters(_testName), duration + "ms"));
        }

        public void WriteTestPassed(TimeSpan duration)
        {
            WriteTestFinished(duration.TotalMilliseconds);            
        }

        public void WriteTestIgnored(string message)
        {
            Console.WriteLine(String.Format("##teamcity[testIgnored name='{0}' message='{1}']", MessageLogger.EscapeCharacters(_testName),MessageLogger.EscapeCharacters(message)));
            WriteTestFinished(0);
        }

        public void WriteTestFailed(string message, string details)
        {
            Console.WriteLine(String.Format("##teamcity[testFailed name='{0}' message='{1}' details='{2}']", MessageLogger.EscapeCharacters(_testName),
                                            MessageLogger.EscapeCharacters(message), MessageLogger.EscapeCharacters(details)));
            WriteTestFinished(0);
        }
    }
}