using System;
using NUnit.Framework;

namespace FluentBuild.Publishing
{
    [TestFixture]
    public class GoogleCodeTests
    {
        private GoogleCode _subject;
        [SetUp]
        public void SetUp()
        {
            _subject = new GoogleCode();
        }

        [Test]
        public void CreateRequest_ShouldBuildRequest()
        {
            _subject.ProjectName("test");
            var request = _subject.CreateRequest();
            Assert.That(request.RequestUri.ToString(), Is.EqualTo("https://test.googlecode.com/files"));
            Assert.That(request.Method, Is.EqualTo("POST"));
        }

        [Test]
        public void CreateAuthorizationToken_ShouldNotFail()
        {
            var authorizationToken = GoogleCode.CreateAuthorizationToken("test", "test");
        }

        [Test, ExpectedException(typeof(ArgumentNullException))]
        public void ValidateString_ShouldThrowExceptionWhenStringIsEmpty()
        {
            _subject.ValidateString(null, "Error");
        }

        [Test]
        public void Validate_ShouldNotThrowException()
        {
            
            _subject.LocalFileName("c:\\tmp.txt")
                .Password("pass")
                .ProjectName("proj")
                .Summary("summary")
                .TargetFileName("targetName")
                .UserName("username").Validate();
        }
    }
}