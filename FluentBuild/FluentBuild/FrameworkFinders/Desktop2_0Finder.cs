using System;
using FluentBuild.Utilities;

namespace FluentBuild.FrameworkFinders
{
    ///<summary>
    /// Determines the location of Framework 2.0 components.
    ///</summary>
    public class Desktop2_0Finder : DefaultFinder
    {
        ///<summary>
        /// Creates the finder.
        ///</summary>
        public Desktop2_0Finder()
        {   
            PossibleSdkInstallKeys.Add(@"SOFTWARE\Microsoft\.NETFramework\sdkInstallRootv2.0");
        }

        protected internal override string FrameworkFolderVersionName
        {
            get { return "v2.0.50727"; }
        }
    }
}