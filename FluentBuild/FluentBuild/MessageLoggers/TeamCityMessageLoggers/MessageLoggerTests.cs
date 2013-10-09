using System;
using System.Collections.Specialized;
using FluentBuild.MessageLoggers.TeamCityMessageLoggers;
using NUnit.Framework;

namespace FluentBuild.MessageLoggers
{
    [TestFixture]
    public class MessageLoggerTests
    {
        #region Setup/Teardown

        [SetUp]
        public void Setup()
        {
            _textMessageWriter = new TextMessageWriter();
            Console.SetOut(_textMessageWriter);
            _subject = new MessageLogger();
        }

        #endregion

        private MessageLogger _subject;
        private TextMessageWriter _textMessageWriter;

        [Test]
        public void WriteTestSuiteStarted_ShouldCreateNewLoggingObject()
        {
            var testSuiteMessageLogger = _subject.WriteTestSuiteStarted("na");
            Assert.That(testSuiteMessageLogger, Is.TypeOf<TeamCityMessageLoggers.TestSuiteMessageLogger>());
        }

        [Test]
        public void WriteHeader_ShouldOpenNewHeaderIfNothingElseHasBeenOpened()
        {
            var header = "test";
            _subject.WriteHeader(header);
            Assert.That(_textMessageWriter.ToString(), Is.EqualTo(String.Format("##teamcity[blockOpened name='{0}']" + Environment.NewLine, header)));            
        }

        [Test]
        public void Write_ShouldCreateMessage()
        {   
            _subject.Write("test", "this is a test");
            Assert.That(_textMessageWriter.ToString(), Is.EqualTo("##teamcity[message text='[test|] this is a test' errorDetails='' status='NORMAL']\r\n"));
        }

        [Test]
        public void WriteWarning_ShouldCreateMessage()
        {
            _subject.WriteWarning("TEST", "this is a test");
            Assert.That(_textMessageWriter.ToString(), Is.EqualTo("##teamcity[message text='[TEST|] this is a test' errorDetails='' status='WARNING']\r\n"));
        }

        [Test]
        public void Debug_ShouldCreateMessage()
        {
            _subject.WriteDebugMessage("this is a test");
            Assert.That(_textMessageWriter.ToString(), Is.EqualTo("##teamcity[message text='[DEBUG|] this is a test' errorDetails='' status='NORMAL']\r\n"));
        }

        [Test]
        public void Error_ShouldCreateMessage()
        {
            _subject.WriteError("CSC", "this is a test");
            Assert.That(_textMessageWriter.ToString(), Is.EqualTo("##teamcity[message text='this is a test' errorDetails='this is a test' status='ERROR']\r\n"));
        }

        [Test]
        public void WriteHeader_ShouldCloseOldHeaderIfItExists()
        {
            var newHeader = "NewHeader";
            var oldHeader = "OldHeader";
            _subject._currentHeader = oldHeader;
            _subject.WriteHeader(newHeader);
            var expected = String.Format("##teamcity[blockClosed name='{0}']", oldHeader) +
                           Environment.NewLine +
                           String.Format("##teamcity[blockOpened name='{0}']", newHeader) +
                           Environment.NewLine;
            Assert.That(_textMessageWriter.ToString(), Is.EqualTo(expected));
        }


        [Test]
        public void EscapeCharacters_ShouldEscapeCharacters()
        {
            var itemsToTest = new NameValueCollection();

            itemsToTest.Add("|", "||");
            itemsToTest.Add("'", "|'");
            itemsToTest.Add("\n", "|n");
            itemsToTest.Add("\r", "|r");
            itemsToTest.Add("]", "|]");

            foreach (string key in itemsToTest.Keys)
            {
                Assert.That(MessageLogger.EscapeCharacters(key), Is.EqualTo(itemsToTest[key]));
            }
        }

        [Test]
        public void EscapeCharacters_ShouldNotEscapePipeRepeatedly()
        {
            string escapeCharacters = MessageLogger.EscapeCharacters("\n|");
            Assert.That(escapeCharacters, Is.EqualTo("|n||"));
        }
    }
}