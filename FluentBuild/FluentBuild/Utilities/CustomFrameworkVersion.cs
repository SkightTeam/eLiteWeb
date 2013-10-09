//namespace FluentBuild.Utilities
//{
    
//    public class CustomFrameworkVersion
//    {
//        private string _sdkPath;
//        private string _frameworkPath;

//        public CustomFrameworkVersion SdkRegistryKeyLocatedAt(string key)
//        {
//            _sdkPath = key;
//            return this;
//        }

//        public CustomFrameworkVersion FrameworkPathLocatedAt(string key)
//        {
//            _frameworkPath = key;
//            return this;
//        }

//        public IFrameworkVersion Create()
//        {
            
//            return new FrameworkVersion("", new[] {_sdkPath}, new[] {_frameworkPath } );
//        }
//        //FrameworkVersion.Custom.Sdk.FindViaRegistryKey(key).FrameworkPath.FindViaRegistryKey(key)

//    }
//}