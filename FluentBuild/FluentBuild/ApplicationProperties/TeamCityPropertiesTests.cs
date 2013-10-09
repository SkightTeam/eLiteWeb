using FluentBuild.Utilities;
using NUnit.Framework;
using Rhino.Mocks;

namespace FluentBuild.ApplicationProperties
{
    ///<summary>
    ///</summary>
    ///<summary />
    [TestFixture]
    public class TeamCityPropertiesTests
    {
        #region Setup/Teardown

        ///<summary>
        ///</summary>
        ///<summary />
        [SetUp]
        public void Setup()
        {
            _environmentVariableWrapper = MockRepository.GenerateStub<IEnvironmentVariableWrapper>();
            _subject = new TeamCityProperties(_environmentVariableWrapper);
        }

        #endregion

        private IEnvironmentVariableWrapper _environmentVariableWrapper;
        private TeamCityProperties _subject;

        ///<summary>
        ///</summary>
        ///<summary />
        [Test]
        public void BuildNumberShouldCallToWrapper()
        {
            string buildNumber = _subject.BuildNumber;
            _environmentVariableWrapper.AssertWasCalled(x => x.Get(Arg<string>.Is.Anything));
        }

        ///<summary>
        ///</summary>
        ///<summary />
        [Test]
        public void ConfigurationNameShouldCallToWrapper()
        {
            string data = _subject.ConfigurationName;
            _environmentVariableWrapper.AssertWasCalled(x => x.Get(Arg<string>.Is.Anything));
        }

        ///<summary>
        ///</summary>
        ///<summary />
        [Test]
        public void ProjectName()
        {
            string data = _subject.ProjectName;
            _environmentVariableWrapper.AssertWasCalled(x => x.Get(Arg<string>.Is.Anything));
        }

        [Test]
        public void TeamCityVersion()
        {
            string data = _subject.TeamCityVersion;
            _environmentVariableWrapper.AssertWasCalled(x => x.Get(Arg<string>.Is.Anything));
        }

        [Test]
        public void BuildConfigurationName()
        {
            string data = _subject.BuildConfigurationName;
            _environmentVariableWrapper.AssertWasCalled(x => x.Get(Arg<string>.Is.Anything));
        }

        [Test]
        public void BuildVersionControlSystemNumber()
        {
            string data = _subject.BuildVersionControlSystemNumber("test");
            _environmentVariableWrapper.AssertWasCalled(x => x.Get(Arg<string>.Is.Anything));
        }

        [Test]
        public void GetProperty()
        {
            string data = _subject.GetProperty("test");
            _environmentVariableWrapper.AssertWasCalled(x => x.Get("test"));
        }


        
    }
}