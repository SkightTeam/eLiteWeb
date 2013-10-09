using System.Collections.Generic;
using System.Linq;
using System.Text;

using FluentBuild.FrameworkFinders;
using NUnit.Framework;
using Rhino.Mocks;

namespace FluentBuild.Utilities
{
    ///<summary />
	[TestFixture]
    public class FrameworkVersionTests
    {

        [Test, ExpectedException(typeof(SdkNotFoundException))]
        public void GetPathToSdk_ShouldThrowExceptionIfNoSdkFound()
        {
            var frameworkFinder = MockRepository.GenerateMock<IFrameworkFinder>();
            
            var framework = new FrameworkVersion("",frameworkFinder);
            frameworkFinder.Stub(x => x.PathToSdk()).Return(null);
            framework.GetPathToSdk();
        }

        ///<summary />
	[Test]
        public void GetPathToSdk_ShouldNotThrowException()
        {
            var frameworkFinder = MockRepository.GenerateMock<IFrameworkFinder>();

            var framework = new FrameworkVersion("", frameworkFinder);
            frameworkFinder.Stub(x => x.PathToSdk()).Return("C:\\temp");
            framework.GetPathToSdk();
        }

        ///<summary />
	[Test]
        public void GetPathToFrameworkInstall_ShouldNotThrowException()
        {
            var frameworkFinder = MockRepository.GenerateMock<IFrameworkFinder>();

            var framework = new FrameworkVersion("", frameworkFinder);
            frameworkFinder.Stub(x => x.PathToFrameworkInstall()).Return("C:\\temp");
            framework.GetPathToFrameworkInstall();
        }

        [Test, ExpectedException(typeof(FrameworkNotFoundException))]
        public void GetPathToFrameworkInstall_ShouldThrowExceptionIfNotFound()
        {
            var frameworkFinder = MockRepository.GenerateMock<IFrameworkFinder>();

            var framework = new FrameworkVersion("", frameworkFinder);
            frameworkFinder.Stub(x => x.PathToFrameworkInstall()).Return(null);
            framework.GetPathToFrameworkInstall();
        }

    }
}
