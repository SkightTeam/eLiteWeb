namespace FluentBuild.Publishing.NuGet
{
    public class ApiKeyMandatory : OptionBase
    {
        public ApiKeyMandatory(NuGetPublisher parent) : base(parent)
        {
        }

        public NuGetOptionals ApiKey(string key)
        {
            _parent._apiKey = key;
            return new NuGetOptionals(_parent);
        }
    }
}