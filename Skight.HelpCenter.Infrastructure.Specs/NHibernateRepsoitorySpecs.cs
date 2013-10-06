using System.Reflection;
using NUnit.Framework;

namespace Skight.HelpCenter.Infrastructure.Specs
{
    [TestFixture]
    public class NHibernateRepositorySpecs {
        [Test]
        [Ignore]
        public void TestCreateScheme() {
            new ApplicationStartup()
            Run.initilize(
                Assembly.GetAssembly(typeof(StartupCommand)),
                Assembly.GetAssembly(typeof(InstallOnNewAPI)));
            SessionProvider provider = SessionProvider.Instance;
            provider.IsBuildScheme = true;
            provider.IsTestMode = true;
            provider.Initilize();
        }
    }
}