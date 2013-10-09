using NUnit.Framework;

namespace FluentBuild.Database
{
    ///<summary />	[TestFixture]
    public class DatabaseTests
    {
        ///<summary />	[Test]
        public void ShouldCreateProperObject()
        {
            Assert.That(Database.MsSqlDatabase, Is.TypeOf(typeof(MsSqlConnection)));
        }
    }
}