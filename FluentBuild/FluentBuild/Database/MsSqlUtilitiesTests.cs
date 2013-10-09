using NUnit.Framework;

using Rhino.Mocks;

namespace FluentBuild.Database
{
    ///<summary />	[TestFixture]
    public class MsSqlUtilitiesTests
    {
        ///<summary />	[Test]
        public void DoesDatabaseAlreadyExist_ShouldCallUnderlyingEngine()
        {
            var engine = MockRepository.GenerateStub<IMsSqlEngine>();
            var subject = new MsSqlUtilities(engine);
            subject.DoesDatabaseAlreadyExist();
            engine.AssertWasCalled(x=>x.DoesDatabaseAlreadyExist());
        }

        ///<summary />	[Test]
        public void CreateOrUpdate_ShouldCreateProperType()
        {
            var engine = MockRepository.GenerateStub<IMsSqlEngine>();
            var subject = new MsSqlUtilities(engine);
            Assert.That(subject.CreateOrUpdate, Is.TypeOf(typeof(MsSqlCreateOrUpdate)));
        }
    }
}