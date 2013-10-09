using System;

namespace FluentBuild.MessageLoggers.TeamCityMessageLoggers
{
    internal class TestSuiteMessageLogger : ITestSuiteMessageLogger
    {
        private readonly string _name;

        public TestSuiteMessageLogger(string name)
        {
            Console.WriteLine(String.Format("##teamcity[testSuiteStarted name='{0}']", MessageLogger.EscapeCharacters(name)));
            _name = name;
        }

        public ITestSuiteMessageLogger WriteTestSuiteStared(string name)
        {
            return new TestSuiteMessageLogger(name);
        }

        public void WriteTestSuiteFinished()
        {
            Console.WriteLine(String.Format("##teamcity[testSuiteFinished name='{0}']", MessageLogger.EscapeCharacters(_name)));
        }

        public ITestLogger WriteTestStarted(string testName)
        {
            Console.WriteLine(String.Format("##teamcity[testStarted name='{0}']", MessageLogger.EscapeCharacters(testName)));
            return new TestMessageLogger(testName);
        }
    }
}