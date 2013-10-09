using System;
using FluentBuild.ApplicationProperties;

namespace FluentBuild
{
    /// <summary>
    /// Deletes the folder if it exists. If it does not exist then no action is taken
    /// </summary>
    /// <returns>The current FluentFs.Core.Directory</returns>
    public static class Properties
    {
        public static ITeamCityProperties TeamCity
        {
            get { return new TeamCityProperties(); }
        }

        public static ICruiseControlProperties CruiseControl
        {
            get { return new CruiseControlProperties(); }
        }

        public static CommandLineProperties CommandLineProperties
        {
            get { return new CommandLineProperties();}
        }

        public static string CurrentDirectory
        {
            get { return Environment.CurrentDirectory; }
        }
    }
}