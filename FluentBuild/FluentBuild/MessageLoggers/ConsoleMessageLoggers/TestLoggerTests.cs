using System;

using NUnit.Framework;

namespace FluentBuild.MessageLoggers.ConsoleMessageLoggers
{
    [TestFixture]
    public class TestLoggerTests
    {
        private TestLogger _subject;
        private TextMessageWriter _textMessageWriter;

        [SetUp]
        public void Setup()
        {
            _textMessageWriter = new TextMessageWriter();

            Console.SetOut(_textMessageWriter);
            ConsoleMessageLogger.WindowWidth = 40;
        }

        [Test]
        public void WriteMessage_ShouldNotAddDotsIfMessageIsTooLong()
        {
            _subject = new TestLogger(0, "");
            _subject.WriteMessage("thisIsReallyLongTestName", "Passed 0.030s");
            Assert.That(_textMessageWriter.ToString(), Is.EqualTo("  [TEST] thisIsReallyLongTestName Passe\r\n         d 0.030s\r\n"));
        }

        [Test]
        public void WriteTestPassed_ShouldOutputTestFinishedMessage()
        {
            _subject = new TestLogger(0, "test1");
            _subject.WriteTestPassed(new TimeSpan(0,0,0,0,30));
            
            Assert.That(_textMessageWriter.ToString(), Is.EqualTo("  [TEST] test1........... Passed 0.030s\r\n"));
        }

        [Test]
        public void WriteTestFailed_ShouldOutputTestFailedAndFinishedMessage()
        {
            _subject = new TestLogger(0,"test1");
            var message = "kaboom";
            var details = "stack trace";
            _subject.WriteTestFailed(message, details);
            var output = String.Format("  [TEST] test1.................. Failed\r\n  [Details] kaboom\r\n  [StackTrace] stack trace\r\n"); 
            Assert.That(_textMessageWriter.ToString(), Is.EqualTo(output));
        }

        [Test]
        public void WriteTestIgnored_ShouldOutputTestIgnoredAndFinishedMessage()
        {
            _subject = new TestLogger(0,"test1");
            var message = "kaboom";
            _subject.WriteTestIgnored(message);
            var output = String.Format("  [TEST] test1................. Ignored\r\n");
            Assert.That(_textMessageWriter.ToString(), Is.EqualTo(output));
        }
    }
}