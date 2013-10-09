using System;
using NUnit.Framework;

namespace FluentBuild.MessageLoggers.TeamCityMessageLoggers
{
    [TestFixture]
    public class TestSuiteMessageLoggerTests
    {
        private TextMessageWriter _textMessageWriter;
        private TestSuiteMessageLogger _subject;
        private string _suiteStartedMessage;

        [SetUp]
        public void Setup()
        {
            _textMessageWriter = new TextMessageWriter();
            Console.SetOut(_textMessageWriter);
            _subject = new TestSuiteMessageLogger("test");
            _suiteStartedMessage = "##teamcity[testSuiteStarted name='test']\r\n";
            Assert.That(_textMessageWriter.ToString(), Is.EqualTo(_suiteStartedMessage));
        }

        [Test]
        public void WriteFinished_ShouldWriteFinishedMessage()
        {
            _subject.WriteTestSuiteFinished();
            Assert.That(_textMessageWriter.ToString(), Is.EqualTo(_suiteStartedMessage+ "##teamcity[testSuiteFinished name='test']\r\n"));   
        }

        [Test]
        public void WriteTestStarted_ShouldWriteStartedMessage()
        {
            _subject.WriteTestStarted("testName");
            Assert.That(_textMessageWriter.ToString(), Is.EqualTo(_suiteStartedMessage+ "##teamcity[testStarted name='testName']\r\n"));
        }

        [Test]
        public void WriteTestSuiteStarted_ShouldWriteStartedMessage()
        {
            _subject.WriteTestSuiteStared("childSuite");
            Assert.That(_textMessageWriter.ToString(), Is.EqualTo(_suiteStartedMessage + "##teamcity[testSuiteStarted name='childSuite']\r\n"));
        }
    }
}