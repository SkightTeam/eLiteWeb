using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace FluentBuild.MessageLoggers.ConsoleMessageLoggers
{
    ///<summary />
    [TestFixture]
    public class ConsoleMessageLoggerTests
    {
        #region Setup/Teardown

        ///<summary />
        [SetUp]
        public void Setup()
        {
            _textMessageWriter = new TextMessageWriter();
            _messageLogger = new ConsoleMessageLogger();
            Console.SetOut(_textMessageWriter);
        }

        #endregion

        private TextMessageWriter _textMessageWriter;
        private ConsoleMessageLogger _messageLogger;

       

       

        [Test]
        public void WrapText_ShouldHandleNewLines()
        {
            ConsoleMessageLogger.WindowWidth = 22;
            List<string> wrapText = _messageLogger.WrapText(9, "  [exec] hello world how \r\nare you");
            Assert.That(wrapText[0], Is.EqualTo("  [exec] hello world "));
            Assert.That(wrapText[1], Is.EqualTo("         how"));
            Assert.That(wrapText[2], Is.EqualTo("         are you"));
        }

        [Test]
        public void WrapText_ShouldHaveOneLine()
        {
            ConsoleMessageLogger.WindowWidth = 22;
            List<string> wrapText = _messageLogger.WrapText(0, "hello");
            Assert.That(wrapText[0], Is.EqualTo("hello"));
        }

        [Test]
        public void WrapText_ShouldHaveTwoLines()
        {
            ConsoleMessageLogger.WindowWidth = 22;
            List<string> wrapText = _messageLogger.WrapText(9, "  [exec] hello world how are you");
            Assert.That(wrapText[0], Is.EqualTo("  [exec] hello world "));
            Assert.That(wrapText[1], Is.EqualTo("         how are you"));
        }
        
        ///<summary />
        [Test]
        public void WriteHeader()
        {
            _messageLogger.WriteHeader("test");
            Assert.That(_textMessageWriter.ToString(), Is.EqualTo("test" + Environment.NewLine));
        }

        ///<summary />
        [Test]
        public void Write_ShouldCreateProperlyIndentedLines()
        {
            ConsoleMessageLogger.WindowWidth = 200;
            _messageLogger.Write("TEST", "Content of message");
            Assert.That(_textMessageWriter.ToString(), Is.EqualTo("  [TEST] Content of message" + Environment.NewLine));
        }

        [Test]
        public void WriteError_ShouldWriteErrorMessage()
        {
            ConsoleMessageLogger.WindowWidth = 200;
            _messageLogger.WriteError("ERROR", "Content of message");
            Assert.That(_textMessageWriter.ToString(), Is.EqualTo("  [ERROR] Content of message" + Environment.NewLine));
        }

        [Test]
        public void WriteDebug_ShouldWriteErrorMessage()
        {
            ConsoleMessageLogger.WindowWidth = 200;
            _messageLogger.WriteDebugMessage("Content of message");
            Assert.That(_textMessageWriter.ToString(), Is.EqualTo("  [DEBUG] Content of message" + Environment.NewLine));
        }

        [Test]
        public void WriteWarning_ShouldWriteMessage()
        {
            ConsoleMessageLogger.WindowWidth = 200;
            _messageLogger.WriteWarning("CSC", "Content of message");
            Assert.That(_textMessageWriter.ToString(), Is.EqualTo("  [CSC] Content of message" + Environment.NewLine));
        }

        [Test]
        public void WriteTestSuiteStarted_ShouldCreateTestSuiteObject()
        {
            var testSuiteMessageLogger = _messageLogger.WriteTestSuiteStarted("test");
            Assert.That(testSuiteMessageLogger, Is.TypeOf<TestSuiteLogger>());
        }

        [Test]
        public void WrapText_ShouldWrapShortHeader()
        {
            ConsoleMessageLogger.WindowWidth = 40;
            var wrapText = _messageLogger.WrapText(0, "short\r\nthis is the message body it is quite long and should wrap too");
            Assert.That(wrapText[0], Is.EqualTo("short"));
        }
    }
}