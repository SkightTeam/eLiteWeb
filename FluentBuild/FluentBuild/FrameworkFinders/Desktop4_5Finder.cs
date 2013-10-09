namespace FluentBuild.FrameworkFinders
{
    public class Desktop4_5Finder : DefaultFinder
    {
        ///<summary>
        /// Creates the finder.
        ///</summary>
        public Desktop4_5Finder()
        {
            PossibleSdkInstallKeys.Add(@"SOFTWARE\Microsoft\Microsoft SDKs\Windows\v8.0A\InstallationFolder");
            PossibleSdkInstallKeys.Add(@"SOFTWARE\Microsoft\Microsoft SDKs\Windows\v8.0\InstallationFolder");
        }

        protected internal override string FrameworkFolderVersionName
        {
            get { return "v4.0.30319"; }
        }
    }
}