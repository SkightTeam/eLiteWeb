using System.Collections.Generic;
using NUnit.Framework;

namespace FluentBuild.UtilitySupport
{
    [TestFixture]
    public class CompileServiceTests
    {
        public class Sample
        {
            public void DoStuff()
            {
                
            }

            public void DoStuff2()
            {
                
            }
        }

        [Test]
        public void ShouldFindAllMethods()
        {
            Assert.That(CompilerService.DoAllMethodsExistInType(typeof (Sample), new List<string>() {"DoStuff", "DoStuff2"}), Is.True);
        }


        [Test]
        public void ShouldNotFindAllMethods()
        {
            Assert.That(CompilerService.DoAllMethodsExistInType(typeof(Sample), new List<string>() { "DoStuff", "DoStuff2", "NonexistantMethod" }), Is.False);
        }

    }
}