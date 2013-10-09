using System;
using NUnit.Framework;

namespace FluentBuild.MessageLoggers.ConsoleMessageLoggers
{
    [TestFixture]
    public class TestSuiteLoggerTests
    {
        private TextMessageWriter _textMessageWriter;
        private TestSuiteLogger _subject;
        private string _suiteStartedMessage;

        [SetUp]
        public void Setup()
        {
            _textMessageWriter = new TextMessageWriter();
            Console.SetOut(_textMessageWriter);
            
            _subject = new TestSuiteLogger(0, "Suite");
            _suiteStartedMessage = "  [TEST] Suite\r\n";
            Assert.That(_textMessageWriter.ToString(), Is.EqualTo(_suiteStartedMessage));
        }

        [Test]
        public void WriteFinished_ShouldNotWriteAnything()
        {
            _subject.WriteTestSuiteFinished();
            Assert.That(_textMessageWriter.ToString(), Is.EqualTo(_suiteStartedMessage));   
        }

        [Test]
        public void WriteTestStarted_ShouldNotWriteAnything()
        {
            _subject.WriteTestStarted("testName");
            Assert.That(_textMessageWriter.ToString(), Is.EqualTo(_suiteStartedMessage));
        }

        [Test]
        public void WriteTestSuiteStarted_ShouldWriteStartedMessage()
        {
            _subject.WriteTestSuiteStared("childSuite");
            Assert.That(_textMessageWriter.ToString(), Is.EqualTo(_suiteStartedMessage + "  [TEST]  childSuite\r\n"));
        }
    }
}