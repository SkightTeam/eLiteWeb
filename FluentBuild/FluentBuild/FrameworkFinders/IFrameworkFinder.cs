namespace FluentBuild.FrameworkFinders
{
    public interface IFrameworkFinder
    {
        string PathToSdk();
        string PathToFrameworkInstall();
        string SdkSearchPathsUsed { get;  }
        string FrameworkSearchPaths { get;  }
    }
}