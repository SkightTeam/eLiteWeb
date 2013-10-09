using System.Xml.Linq;
using FluentBuild.BuildFileConverter.Structure;
using NUnit.Framework;
using Rhino.Mocks;

namespace FluentBuild.BuildFileConverter.Parsing
{
    [TestFixture]
    public class TargetParserTests
    {
        private IParserResolver _resolver;
        private TargetParser _subject;
        private XElement _targetXml;
        private ITargetRepository _targetRepository;

        [SetUp]
        public void Setup()
        {
            _resolver = MockRepository.GenerateStub<IParserResolver>();
            _targetRepository = MockRepository.GenerateStub<ITargetRepository>();
            _subject = new TargetParser(_resolver, _targetRepository);
            _resolver.Stub(x => x.Resolve("call")).Return(new UnkownTypeParser());
            _targetXml = XElement.Parse("<target name=\"basic\" depends=\"clean, compile\"><call target=\"mainbuild\"/></target>");   
        }

        [Test]
        public void ShouldParseDependsSeperatedByCommas()
        {
            var targetXml = XElement.Parse("<target name=\"basic\" depends=\"clean compile\"><call target=\"mainbuild\"/></target>");   
            var target = _subject.Parse(targetXml, null);
            var mockDependancyTarget = MockRepository.GenerateStub<ITarget>();
            _targetRepository.Stub(x => x.Resolve("clean")).Return(mockDependancyTarget);
            _targetRepository.Stub(x => x.Resolve("compile")).Return(mockDependancyTarget);
            Assert.That(target.DependsOn.Count, Is.EqualTo(2));
            _targetRepository.AssertWasCalled(x => x.Register(target));
        }

        [Test]
        public void ShouldParseDepends()
        {
            var target = _subject.Parse(_targetXml, null);
            var mockDependancyTarget = MockRepository.GenerateStub<ITarget>();
            _targetRepository.Stub(x => x.Resolve("clean")).Return(mockDependancyTarget);
            _targetRepository.Stub(x => x.Resolve("compile")).Return(mockDependancyTarget);
            Assert.That(target.DependsOn.Count, Is.EqualTo(2));
            _targetRepository.AssertWasCalled(x=>x.Register(target));
        }

        [Test]
        public void ShouldParseTarget()
        {
            var target = _subject.Parse(_targetXml, null);
            
            Assert.That(target.Name, Is.EqualTo("basic"));
            Assert.That(target.Body, Is.EqualTo(_targetXml.ToString()));
        }

        [Test]
        public void ShouldResolveParser()
        {
            var target = _subject.Parse(_targetXml, null);
            _resolver.AssertWasCalled(x=>x.Resolve("call"));
        }


    }
}