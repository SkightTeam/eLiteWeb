using FluentBuild.BuildFileConverter.Structure;
using NUnit.Framework;

namespace FluentBuild.BuildFileConverter
{
    [TestFixture]
    public class TargetTreeBuilderTests
    {
        [Test]
        public void TestSimpleDependancy()
        {
            var defaultTarget = new Target() { Name="BuildAll"};
            var cleanTarget = new Target() { Name="Clean"};
            defaultTarget.DependsOn.Add(cleanTarget);

            var targets = TargetTreeBuilder.CreateTree(defaultTarget);
            Assert.That(targets[0], Is.EqualTo(cleanTarget));
            Assert.That(targets[1], Is.EqualTo(defaultTarget));
        }

        [Test]
        public void NestedDepencanciesShouldWork()
        {
            //default needs compile which needs clean
            var defaultTarget = new Target() { Name = "BuildAll" };
            var compileTarget = new Target() { Name = "Compile" };
            var cleanTarget = new Target() { Name = "Clean" };
            compileTarget.DependsOn.Add(cleanTarget);

            defaultTarget.DependsOn.Add(compileTarget);

            var targets = TargetTreeBuilder.CreateTree(defaultTarget);
            Assert.That(targets[0], Is.EqualTo(cleanTarget));
            Assert.That(targets[1], Is.EqualTo(compileTarget));
            Assert.That(targets[2], Is.EqualTo(defaultTarget));
        }

        [Test]
        public void TwoDepencanciesOnCleanShouldWork()
        {
            //default needs clean and compile
            //compile also needs clean
            var defaultTarget = new Target() { Name = "BuildAll" };
            var compileTarget = new Target() {Name = "Compile"};
            var cleanTarget = new Target() { Name = "Clean" };
            compileTarget.DependsOn.Add(cleanTarget);

            defaultTarget.DependsOn.Add(cleanTarget);
            defaultTarget.DependsOn.Add(compileTarget);

            var targets = TargetTreeBuilder.CreateTree(defaultTarget);
            Assert.That(targets[0].Name, Is.EqualTo(cleanTarget.Name));
            Assert.That(targets[1].Name, Is.EqualTo(compileTarget.Name));
            Assert.That(targets[2].Name, Is.EqualTo(defaultTarget.Name));
        }
    }
}