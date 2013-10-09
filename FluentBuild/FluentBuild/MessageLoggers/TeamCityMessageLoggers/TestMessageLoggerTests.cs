using System;
using NUnit.Framework;

namespace FluentBuild.MessageLoggers.TeamCityMessageLoggers
{
    [TestFixture]
    public class TestMessageLoggerTests
    {
        private TestMessageLogger _subject;
        private TextMessageWriter _textMessageWriter;

        [SetUp]
        public void Setup()
        {
            _textMessageWriter = new TextMessageWriter();
            Console.SetOut(_textMessageWriter);
            
        }

        [Test]
        public void WriteTestPassed_ShouldOutputTestFinishedMessage()
        {
            _subject = new TestMessageLogger("test1");
            _subject.WriteTestPassed(new TimeSpan(0,0,0,0,30));
            Assert.That(_textMessageWriter.ToString(), Is.EqualTo(GenerateFinishedMessage(30)));
        }

        [Test]
        public void WriteTestFailed_ShouldOutputTestFailedAndFinishedMessage()
        {
            _subject = new TestMessageLogger("test1");
            var message = "kaboom";
            var details = "stack trace";
            _subject.WriteTestFailed(message, details);
            var output = String.Format("##teamcity[testFailed name='test1' message='{0}' details='{1}']\r\n", message,
                                       details) + GenerateFinishedMessage(0); 
            Assert.That(_textMessageWriter.ToString(), Is.EqualTo(output));
        }

        [Test]
        public void WriteTestIgnored_ShouldOutputTestIgnoredAndFinishedMessage()
        {
            _subject = new TestMessageLogger("test1");
            var message = "kaboom";
            _subject.WriteTestIgnored(message);
            var output = String.Format("##teamcity[testIgnored name='test1' message='{0}']\r\n", message) + GenerateFinishedMessage(0);
            Assert.That(_textMessageWriter.ToString(), Is.EqualTo(output));
        }

        private string GenerateFinishedMessage(int duration)
        {
            return String.Format("##teamcity[testFinished name='test1' duration='{0}ms']\r\n", duration);
        }
    }
}