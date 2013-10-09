using NUnit.Framework;
using Rhino.Mocks;

namespace FluentBuild.Database
{
    ///<summary />	[TestFixture]
    public class MsSqlVersionTableTests
    {
        ///<summary />	[Test]
        public void ExecuteShouldCallUnderlyingEngine()
        {
            var engine = MockRepository.GenerateStub<IMsSqlEngine>();
            var subject = new MsSqlVersionTable(engine);
            subject.Execute();
            engine.AssertWasCalled(x=>x.Execute());
        }
    }
}