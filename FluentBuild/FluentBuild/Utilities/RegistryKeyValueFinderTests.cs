using System;
using NUnit.Framework;
using Rhino.Mocks;

namespace FluentBuild.Utilities
{
    ///<summary />
	[TestFixture]
    public class RegistryKeyValueFinderTests
    {
        private IRegistryWrapper _registryWrapper;
        private IRegistryKeyWrapper _registryKey;
        private RegistryKeyValueFinder _subject;
        private string[] _keysToCheck;

        ///<summary />
	    [SetUp]
        public void Setup()
        {
            _registryWrapper = MockRepository.GenerateMock<IRegistryWrapper>();
            _registryKey = MockRepository.GenerateStub<IRegistryKeyWrapper>();
            
            _subject = new RegistryKeyValueFinder(_registryWrapper);
            _keysToCheck = new[] { @"SOFTWARE\App1\SubKey\InstallPath1", @"SOFTWARE\App2\InstallPath2" };

            _registryWrapper.Stub(x => x.OpenLocalMachineKey("SOFTWARE")).Return(_registryKey);
        }
        
        ///<summary />
	    [Test]
        public void ShouldOpenLocalMachineKeyForSoftwareTwice()
        {
            _subject.FindFirstValue(_keysToCheck);
            _registryWrapper.AssertWasCalled(x=>x.OpenLocalMachineKey("SOFTWARE"), c=>c.Repeat.Twice());
        }
    }
}