using System.Reflection;
using NUnit.Framework;
using Skight.HelpCenter.Infrastructure.Persistent;
using Skight.eLiteWeb.Application.Startup;
using Skight.eLiteWeb.Infrastructure.Persistent;

namespace Skight.HelpCenter.Application.Specs
{
    [TestFixture]
    public class NHibernateRepositorySpecs {
        [Test]
        [Ignore]
        public void TestCreateScheme()
        {
            new ApplicationStartup()
                .run(Assembly.GetAssembly(typeof(AnswerMap))); 
            SessionProvider provider = SessionProvider.Instance;
            provider.IsBuildScheme = true;
            provider.IsTestMode = true;
            provider.Initilize();
        }
    }
}