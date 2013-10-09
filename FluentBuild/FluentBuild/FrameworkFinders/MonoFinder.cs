using System;

namespace FluentBuild.FrameworkFinders
{
    public class MonoFinder:DefaultFinder
    {
        public override string PathToSdk ()
        {
            throw new NotImplementedException("SDK support is not available on MONO");
        }

        public override string PathToFrameworkInstall ()
        {
            return "mcs";
        }

        protected internal override string FrameworkFolderVersionName {
            get { throw new NotImplementedException ("Not implemented by mono"); }
        }
    }
}