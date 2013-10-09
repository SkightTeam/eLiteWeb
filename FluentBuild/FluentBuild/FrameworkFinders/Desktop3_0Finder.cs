using FluentBuild.Utilities;

namespace FluentBuild.FrameworkFinders
{
    ///<summary>
    /// Determines the location of Framework 3.0 components.
    ///</summary>
    public class Desktop3_0Finder : DefaultFinder
    {
        ///<summary>
        /// Creates the finder.
        ///</summary>
        public Desktop3_0Finder()
        {
            PossibleSdkInstallKeys.Add(@"SOFTWARE\Microsoft\.NETFramework\sdkInstallRootv2.0");
        }

        protected internal override string FrameworkFolderVersionName
        {
            //this is done as 3.0 uses the 2.0 compiles with some new libraries
            get { return "v2.0.50727"; }
        }

    }
}