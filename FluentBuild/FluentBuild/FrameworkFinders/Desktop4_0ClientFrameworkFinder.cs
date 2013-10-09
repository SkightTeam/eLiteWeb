namespace FluentBuild.FrameworkFinders
{
    ///<summary>
    /// Determines the location of Framework 4.0 client components.
    ///</summary>
    public class Desktop4_0ClientFrameworkFinder : DefaultFinder
    {
        ///<summary>
        /// Creates the finder.
        ///</summary>
        public Desktop4_0ClientFrameworkFinder()
        {
            PossibleSdkInstallKeys.Add(@"SOFTWARE\Microsoft\Microsoft SDKs\Windows\v7.0A\InstallationFolder");
            PossibleSdkInstallKeys.Add(@"SOFTWARE\Microsoft\Microsoft SDKs\Windows\v7.0\InstallationFolder");

            PossibleFrameworkInstallKeys.Add(@"SOFTWARE\Microsoft\NET Framework Setup\NDP\v4\Client\InstallPath");

        }

        protected internal override string FrameworkFolderVersionName
        {
            get { return "v4.0.30319"; }
        }
    }
}