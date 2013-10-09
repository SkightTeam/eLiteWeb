using System;

namespace FluentBuild.Publishing.NuGet
{
    public class VersionMandatory : OptionBase
    {
        public VersionMandatory(NuGetPublisher parent) : base(parent) { }

        public DescriptionMandatory Version(Version version)
        {
            return Version(version.ToString());
        }

        public DescriptionMandatory Version(string version)
        {
            _parent._version = version;
            return new DescriptionMandatory(_parent);
        }
    }
}