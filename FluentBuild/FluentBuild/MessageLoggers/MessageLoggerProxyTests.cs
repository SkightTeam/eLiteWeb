using System;
using FluentBuild.MessageLoggers.ConsoleMessageLoggers;
using NUnit.Framework;
using Rhino.Mocks;

namespace FluentBuild.MessageLoggers
{
    [TestFixture]
    public class MessageLoggerProxyTests
    {
        private IMessageLogger _internalLogger;
        private MessageLoggerProxy _messageLoggerProxy;

        ///<summary />
        [SetUp]
        public void Setup()
        {
            _internalLogger = MockRepository.GenerateMock<IMessageLogger>();
            _messageLoggerProxy = ((MessageLoggerProxy)Defaults.Logger);
            _messageLoggerProxy.InternalLogger = _internalLogger;
        }


        [Test]
        public void UsingDebug_ShouldOnlyWriteOneDebugMessage()
        {
            _messageLoggerProxy.Verbosity = VerbosityLevel.TaskDetails;
            _messageLoggerProxy.WriteDebugMessage("test1");
            using (_messageLoggerProxy.ShowDebugMessages)
            {
                _messageLoggerProxy.WriteDebugMessage("test2");
            }
            _messageLoggerProxy.InternalLogger.AssertWasCalled(x=>x.WriteDebugMessage("test2"));
        }

        [Test]
        public void UsingDebug_DebugLevelsSholdChange()
        {
            _messageLoggerProxy.Verbosity = VerbosityLevel.TaskDetails;
            using (_messageLoggerProxy.ShowDebugMessages)
            {
                Assert.That(_messageLoggerProxy.Verbosity, Is.EqualTo(VerbosityLevel.Full));
            }
            Assert.That(_messageLoggerProxy.Verbosity, Is.EqualTo(VerbosityLevel.TaskDetails));
        }

        ///<summary />
        [Test]
        public void WriteDebugMessage_ShouldNotWriteIfVerbosityIsLessThanFull()
        {
            _messageLoggerProxy.Verbosity = VerbosityLevel.TaskDetails;
            _messageLoggerProxy.WriteDebugMessage("test");
            _messageLoggerProxy.InternalLogger.AssertWasNotCalled(x=>x.WriteDebugMessage(Arg<String>.Is.Anything));
        }

        ///<summary />
        [Test]
        public void WriteDebugMessage_ShouldWriteIfVerbosityIsFull()
        {
            _messageLoggerProxy.Verbosity = VerbosityLevel.Full;
            _messageLoggerProxy.WriteDebugMessage("test");
            _internalLogger.AssertWasCalled(x=>x.WriteDebugMessage("test"));
        }

        [Test]
        public void WriteHeader_ShouldWriteIfVerbosityIsFull()
        {
            _messageLoggerProxy.Verbosity = VerbosityLevel.Full;
            _messageLoggerProxy.WriteHeader("test");
            _messageLoggerProxy.InternalLogger.AssertWasCalled(x => x.WriteHeader("test"));
        }

        [Test]
        public void WriteHeader_ShouldNotWriteIfVerbosityIsNone()
        {
            _messageLoggerProxy.Verbosity = VerbosityLevel.None;
            _messageLoggerProxy.WriteHeader("test");
            _messageLoggerProxy.InternalLogger.AssertWasNotCalled(x => x.WriteHeader("test"));
        }

        [Test]
        public void WriteError_ShouldWriteIfVerbosityIsNone()
        {
            _messageLoggerProxy.Verbosity = VerbosityLevel.None;
            _messageLoggerProxy.WriteError("ERROR", "test");
            _internalLogger.AssertWasCalled(x => x.WriteError("ERROR", "test"));
        }

        [Test]
        public void WriteError_ShouldWriteCustomPrefix()
        {
            _messageLoggerProxy.Verbosity = VerbosityLevel.None;
            _messageLoggerProxy.WriteError("ERR", "test");
            _messageLoggerProxy.InternalLogger.AssertWasCalled(x => x.WriteError("ERR", "test"));
        }


        [Test]
        public void WriteWarning_ShouldWriteCustomPrefix()
        {
            _messageLoggerProxy.Verbosity = VerbosityLevel.Full;
            _messageLoggerProxy.WriteWarning("WRN", "test");
            _messageLoggerProxy.InternalLogger.AssertWasCalled(x => x.WriteWarning("WRN", "test"));
        }

        [Test]
        public void SetLogger_ShouldSetLoggerToConsoleLogger()
        {
            Defaults.SetLogger("console");
            Assert.That(((MessageLoggerProxy)Defaults.Logger).InternalLogger, Is.TypeOf<ConsoleMessageLogger>());
        }

        [Test]
        public void SetLogger_ShouldSetLoggerToTeamCityLogger()
        {
            Defaults.SetLogger("TeamCity");
            Assert.That(((MessageLoggerProxy)Defaults.Logger).InternalLogger, Is.TypeOf<MessageLoggers.TeamCityMessageLoggers.MessageLogger>());
        }

        [Test, ExpectedException(typeof(ArgumentException))]
        public void SetLogger_ShouldThrowExceptionIfUnkownType()
        {
            Defaults.SetLogger("garbage");
        }

        [Test]
        public void WriteTestStarted_ShouldCallInternalLogger()
        {
            var name = "test";
            _messageLoggerProxy.WriteTestSuiteStarted(name);
            _messageLoggerProxy.InternalLogger.AssertWasCalled(x=>x.WriteTestSuiteStarted(name));
        }

        [TearDown]
        public void ResetLogger()
        {
            Defaults.SetLogger("CONSOLE");
        }
    }
}