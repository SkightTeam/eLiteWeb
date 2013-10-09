namespace FluentBuild.Publishing.NuGet
{
    public class OptionBase
    {
        internal readonly NuGetPublisher _parent;

        public OptionBase(NuGetPublisher parent)
        {
            _parent = parent;
        }
    }
}