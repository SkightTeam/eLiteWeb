using System;
using NUnit.Framework;

namespace FluentBuild.Publishing.NuGet
{
    [TestFixture]
    public class AuthorsMandatoryTests
    {
        [Test]
        public void ShouldSetAuthor()
        {
            var nuGetPublisher = new NuGetPublisher();
            var subject = new AuthorsMandatory(nuGetPublisher);
            var result = subject.Author("author");
            Assert.That(nuGetPublisher._authors, Is.EqualTo("author"));
            Assert.That(result, Is.Not.Null);
            Assert.That(result._parent, Is.EqualTo(nuGetPublisher));
        }

        [Test]
        public void ShouldSetMultipleAuthors()
        {
            var nuGetPublisher = new NuGetPublisher();
            var subject = new AuthorsMandatory(nuGetPublisher);
            var result = subject.Authors("author1", "author2");
            Assert.That(nuGetPublisher._authors, Is.EqualTo("author1, author2"));
            Assert.That(result, Is.Not.Null);
            Assert.That(result._parent, Is.EqualTo(nuGetPublisher));
        }

        [Test, ExpectedException(typeof(ArgumentException))]
        public void ShouldThrowExceptionIfNoAuthorPassedIn()
        {
            var subject = new AuthorsMandatory(null);
            var result = subject.Authors();
        }
    }
}