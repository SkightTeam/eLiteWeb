using System;
using System.Collections.Generic;

using NUnit.Framework;
using Rhino.Mocks;

namespace FluentBuild.MessageLoggers.MessageProcessing
{
    [TestFixture]
    public class DefaultMessageProcessorTests
    {
        #region Setup/Teardown

        [SetUp]
        public void Setup()
        {
            _internalLogger = MockRepository.GenerateStub<IMessageLogger>();
           ((MessageLoggerProxy)Defaults.Logger).InternalLogger = _internalLogger;
            _subject = new DefaultMessageProcessor();
        }

        #endregion

        private DefaultMessageProcessor _subject;
        private IMessageLogger _internalLogger;

        [Test]
        public void ShouldProcessErrorMessage()
        {
            var messages = new List<Message>();
            messages.Add(new Message("TEST", MessageType.Error));
            _subject.Display(messages);
           _internalLogger.AssertWasCalled(x => x.WriteError(Arg<String>.Is.Anything, Arg<String>.Is.Anything));
        }

        [Test]
        public void ShouldProcessWarningMessage()
        {
            var messages = new List<Message>();
            messages.Add(new Message("TEST", MessageType.Warning));
            _subject.Display(messages);
            _internalLogger.AssertWasCalled(x => x.WriteWarning(Arg<String>.Is.Anything, Arg<String>.Is.Anything));
        }

        [Test]
        public void ShouldProcessRegularMessage()
        {
            var messages = new List<Message>();
            messages.Add(new Message("TEST", MessageType.Regular));
            _subject.Display(messages);
            _internalLogger.AssertWasCalled(x => x.Write(Arg<String>.Is.Anything, Arg<String>.Is.Anything, Arg<String[]>.Is.Anything));
        }

        [Test, ExpectedException(typeof(NotImplementedException))]
        public void ShouldNotProcessUnkownErrorType()
        {
            var messages = new List<Message>();
            messages.Add(new Message("TEST", (MessageType)999));
            _subject.Display(messages);
            _internalLogger.AssertWasNotCalled(x => x.Write(Arg<String>.Is.Anything, Arg<String>.Is.Anything));
        }

        [Test]
        public void ShouldHaveAllMessagesAsErrorWhenProcessErrorCodeNonZero()
        {
            IList<Message> messages = _subject.Parse("prefix", "I did something", "", 1);
            Assert.That(messages[0].MessageType, Is.EqualTo(MessageType.Error));
        }

        [Test]
        public void ShouldHaveMessagesAsRegular()
        {
            IList<Message> messages = _subject.Parse("prefix", "I did something", "", 0);
            Assert.That(messages[0].MessageType, Is.EqualTo(MessageType.Regular));
        }

        [Test]
        public void ShouldHaveNonErrorAndErrorMessage()
        {
            IList<Message> messages = _subject.Parse("prefix", "I did something", "I failed on something", 0);
            Assert.That(messages[0].MessageType, Is.EqualTo(MessageType.Regular));
            Assert.That(messages[1].MessageType, Is.EqualTo(MessageType.Error));
        }

        [Test]
        public void ShouldHaveWarningLine()
        {
            IList<Message> messages =
                _subject.Parse("prefix", "I did something" + Environment.NewLine + "Warning: something is broken", "", 0);
            Assert.That(messages[0].MessageType, Is.EqualTo(MessageType.Regular));
            Assert.That(messages[1].MessageType, Is.EqualTo(MessageType.Warning));
        }
    }
}