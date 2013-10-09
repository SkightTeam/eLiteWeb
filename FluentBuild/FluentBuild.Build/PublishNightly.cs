using FluentBuild;


namespace Build
{
    public class PublishNightly : Publish
    {
        public PublishNightly()
        {
            _version = Properties.CommandLineProperties.GetProperty("Version");
            _finalFileName = "FluentBuild-Nightly-" + _version + ".zip";
        }
    }
}