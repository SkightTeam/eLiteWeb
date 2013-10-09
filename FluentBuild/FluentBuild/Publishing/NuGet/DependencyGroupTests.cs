using System;
using NUnit.Framework;

namespace FluentBuild.Publishing.NuGet
{
    [TestFixture]
    public class DependencyGroupTests
    {
        [Test]
        public void ShouldGenerateXmlWithoutFramwork()
        {
            var subject = new DependencyGroup();
            subject.Add("Project1");
            Assert.That(subject.ToString(), Is.EqualTo(string.Format("<group>{0}<dependency id=\"{1}\" />{0}</group>{0}", Environment.NewLine, "Project1")));
        }

        [Test]
        public void ShouldGenerateXmlWithFramwork()
        {
            var subject = new DependencyGroup();
            subject.Framework = "net40";
            subject.Add("Project1");
            Assert.That(subject.ToString(), Is.EqualTo(string.Format("<group targetFramework=\"{1}\">{0}<dependency id=\"{2}\" />{0}</group>{0}", Environment.NewLine, subject.Framework, "Project1")));
        }

        [Test]
        public void ShouldAddWithVersion()
        {
            var subject = new DependencyGroup();
            subject.Add("Project1", "[1,3)");
            Assert.That(subject.Depenencies.Count, Is.EqualTo(1));
        }
    }
}