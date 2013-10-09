using System.Collections.Generic;
using System.Linq;
using FluentBuild.Utilities;

namespace FluentBuild.FrameworkFinders
{
    //check all folders in
    //pull Version
    //greater than 3.5 can pull InstallPath from folder
    //highest version wins.

    //seek out SDK for set version.

    //if information can not be found then fail?
    //SDK is not required for msbuild so maybe failing should be done only when the 
    //property is used.
    //SDK has sn, xsd.exe, wsdl.exe, svcutil.exe, resgen.exe

    //32 bit vs 64

    //need custom framework version so that people can implement it before an official release is out

    //Silverlight:
    //C:\Program Files (x86)\Microsoft Silverlight\4.0.50826.0

    //need friendly name
    //sdkdirectory
    //framework directoy
    //clr version#

    //FrameworkVersion.Desktop.v1_1.ServicePack.1
    //FrameworkVersion.Silverlight
    //FrameworkVersion.Compact
    //FrameworkVersion.Mono / Moonlight

    ///<summary>
    ///Default abstract finder class. Used to do common work for finding frameworks.
    ///</summary>
    public abstract class DefaultFinder : IFrameworkFinder
    {
        private readonly IRegistryKeyValueFinder _finder;

        protected DefaultFinder() : this(new RegistryKeyValueFinder())
        {
            
        }

        protected IList<string> PossibleSdkInstallKeys { get; set; }

        protected internal abstract string FrameworkFolderVersionName { get; }
        protected IList<string> PossibleFrameworkInstallKeys { get; set; }

        #region IFrameworkFinder Members

        protected DefaultFinder(IRegistryKeyValueFinder finder)
        {
            _finder = finder;
            PossibleSdkInstallKeys = new List<string>();
            PossibleFrameworkInstallKeys = new List<string>();
        }

        ///<summary>
        /// Seaches the registry for a given framework and determines the most
        /// accurate physical path to the SDK
        ///</summary>
        ///<returns>Path to SDK if found. Null if not found</returns>
        public virtual string PathToSdk()
        {
            KeyValuePair<string, string> foundValue = _finder.FindFirstValue(PossibleSdkInstallKeys.ToArray());
            if (string.IsNullOrEmpty(foundValue.Key))
                return null;
            return foundValue.Value;
        }

        ///<summary>
        /// Seaches the registry for a given framework and determines the most
        /// accurate physical path to the framework
        ///</summary>
        ///<returns>Path to framework if found. Null if not found</returns>
        public virtual string PathToFrameworkInstall()
        {
            //TODO: this should be moved out as FrameworkSearchPaths will not include this
            string baseInstallPath = @"SOFTWARE\Microsoft\.NETFramework\InstallRoot";
            PossibleFrameworkInstallKeys.Add(baseInstallPath);
            KeyValuePair<string, string> foundValue = _finder.FindFirstValue(PossibleFrameworkInstallKeys.ToArray());
            if (string.IsNullOrEmpty(foundValue.Key))
                return null;
            if (foundValue.Key == @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\.NETFramework")
                return foundValue.Value + "\\" + FrameworkFolderVersionName;
            return foundValue.Value;
        }


        ///<summary>
        /// Creates a comma seperated list of paths used to find the SDK.
        /// This is typically used to generate error message text
        /// when the SDK can not be found
        ///</summary>
        public string SdkSearchPathsUsed
        {
            get { return StringUtility.CreateCommaSeperatedList(PossibleSdkInstallKeys); }
        }

        ///<summary>
        /// Creates a comma seperated list of paths used to find the SDK.
        /// This is typically used to generate error message text
        /// when the framework can not be found
        ///</summary>
        public string FrameworkSearchPaths
        {
            get { return StringUtility.CreateCommaSeperatedList(PossibleFrameworkInstallKeys); }
        }

        #endregion
    }
}