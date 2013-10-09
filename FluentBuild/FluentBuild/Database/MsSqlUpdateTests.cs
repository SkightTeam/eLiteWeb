using NUnit.Framework;

using Rhino.Mocks;

namespace FluentBuild.Database
{
    ///<summary />	[TestFixture]
    public class MsSqlUpdateTests
    {
        ///<summary />	[Test]
        public void VersionTable_ShouldSetVersionOnEngine()
        {
            var engine = MockRepository.GenerateMock<IMsSqlEngine>();
            var subject = new MsSqlUpdate(engine);
            var msSqlVersionTable = subject.VersionTable("blah");
            Assert.That(msSqlVersionTable, Is.TypeOf(typeof(MsSqlVersionTable)));
            engine.AssertWasCalled(x=>x.VersionTable="blah");
        }
    }
}