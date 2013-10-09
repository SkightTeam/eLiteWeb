using System;
using FluentBuild.Utilities;

namespace FluentBuild.ApplicationProperties
{
    public interface ITeamCityProperties
    {
        ///<summary>
        /// The BuildNumber that team city has determined for this build
        ///</summary>
        string BuildNumber { get; }

        ///<summary>
        /// The configuration being used for this build
        ///</summary>
        string ConfigurationName { get; }

        ///<summary>
        /// The project being built
        ///</summary>
        string ProjectName { get; }

        ///<summary>
        /// The version of TeamCity
        ///</summary>
        string TeamCityVersion { get; }

        ///<summary>
        /// The current build configuration name
        ///</summary>
        string BuildConfigurationName { get; }

        ///<summary>
        /// Gets the latest revision included in the build from the source control system.
        ///</summary>
        ///<param name="simplifiedVcsRootName">The version control root name with any non-alphanumeric characters replaced with a "_"</param>
        ///<returns>The version from the source control system</returns>
        string BuildVersionControlSystemNumber(string simplifiedVcsRootName);

        /// <summary>
        /// Gets a property by name
        /// </summary>
        /// <param name="propertyName">the name of the team city environment variable</param>
        /// <returns>The value of the property</returns>
        string GetProperty(string propertyName);
    }

    ///<summary>
    /// Access common properties set by team city
    ///</summary>
    internal class TeamCityProperties : ITeamCityProperties
    {
        private readonly IEnvironmentVariableWrapper _environmentVariableWrapper;

        internal TeamCityProperties() : this(new EnvironmentVariableWrapper())
        {}

        internal TeamCityProperties(IEnvironmentVariableWrapper environmentVariableWrapper)
        {
            _environmentVariableWrapper = environmentVariableWrapper;
        }

        ///<summary>
        /// The BuildNumber that team city has determined for this build
        ///</summary>
        public string BuildNumber
        {
            get { return GetEnvironmentVariable("BUILD_NUMBER"); }
        }

        ///<summary>
        /// The configuration being used for this build
        ///</summary>
        public string ConfigurationName
        {
            get { return GetEnvironmentVariable("TEAMCITY_BUILDCONF_NAME"); }
        }

        ///<summary>
        /// The project being built
        ///</summary>
        public string ProjectName
        {
            get { return GetEnvironmentVariable("TEAMCITY_PROJECT_NAME"); }
        }

        ///<summary>
        /// The version of TeamCity
        ///</summary>
        public string TeamCityVersion
        {
            get { return GetEnvironmentVariable("TEAMCITY_VERSION"); }
        }

        ///<summary>
        /// The current build configuration name
        ///</summary>
        public string BuildConfigurationName
        {
            get { return GetEnvironmentVariable("TEAMCITY_BUILDCONF_NAME "); }
        }

        ///<summary>
        /// Gets the latest revision included in the build from the source control system.
        ///</summary>
        ///<param name="simplifiedVcsRootName">The version control root name with any non-alphanumeric characters replaced with a "_"</param>
        ///<returns>The version from the source control system</returns>
        public string BuildVersionControlSystemNumber(string simplifiedVcsRootName)
        {
            return GetEnvironmentVariable("BUILD_VCS_NUMBER_" + simplifiedVcsRootName);
        }

        /// <summary>
        /// Gets a property by name
        /// </summary>
        /// <param name="propertyName">the name of the team city environment variable</param>
        /// <returns>The value of the property</returns>
        public string GetProperty(string propertyName)
        {
           return GetEnvironmentVariable(propertyName);
        }

        private string GetEnvironmentVariable(string name)
        {
            return _environmentVariableWrapper.Get(name);
        }
    }
}