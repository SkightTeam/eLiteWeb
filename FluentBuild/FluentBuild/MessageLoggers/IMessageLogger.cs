using System;


namespace FluentBuild.MessageLoggers
{
    public interface IMessageLogger
    {
        void WriteHeader(string header);
        void WriteDebugMessage(string message);
        void Write(string type, string message, params string[] items);
        void WriteError(string type, string message);
        void WriteWarning(string type, string message);
        IDisposable ShowDebugMessages { get; }
        ITestSuiteMessageLogger WriteTestSuiteStarted(string name);
        VerbosityLevel Verbosity { get; set; }
    }

    public interface  ITestSuiteMessageLogger
    {
        ITestSuiteMessageLogger WriteTestSuiteStared(string name);
        void WriteTestSuiteFinished();
        ITestLogger WriteTestStarted(string testName);
        
    }

    public interface ITestLogger
    {
        void WriteTestPassed(TimeSpan duration);
        void WriteTestIgnored(string message);
        void WriteTestFailed(string message, string details);
    }
}