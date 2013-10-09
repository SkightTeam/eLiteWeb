using System;

namespace FluentBuild.Publishing.NuGet
{
    public class NuGetOptionals : OptionBase
    {
        public NuGetOptionals(NuGetPublisher parent) : base(parent)
        {
        }

        public NuGetOptionals Title(string title)
        {
            _parent._title = title;
            return this;
        }

        public NuGetOptionals ReleaseNotes(string releaseNotes)
        {
            _parent._releaseNotes = releaseNotes;
            return this;
        }

        public NuGetOptionals Summary(string summary)
        {
            _parent._summary = summary;
            return this;
        }

        public NuGetOptionals Language(string language)
        {
            _parent._language = language;
            return this;
        }

        public NuGetOptionals AdditionalManifestData(string manifestData)
        {
            _parent._additionalManifestData = manifestData;
            return this;
        }

        public NuGetOptionals LicenseUrl(string url)
        {
            _parent._licenseUrl = url;
            return this;
        }

        public NuGetOptionals Copyright(string copyright)
        {
            _parent._copyright = copyright;
            return this;
        }

        public NuGetOptionals AddDependencyGroup(DependencyGroup dependencyGroup)
        {
            _parent._depenencyGroups.Add(dependencyGroup);
            return this;
        }

        public NuGetOptionals AddReference(string reference)
        {
            _parent._references.Add(reference);
            return this;
        }

        public NuGetOptionals AddFrameworkAssembly(string assembly)
        {
            return AddFrameworkAssembly(assembly, null);
        }

        public NuGetOptionals AddFrameworkAssembly(string assembly, string targetFramework)
        {
            _parent._frameworkAssemblies.Add(new FrameworkAssembly(assembly, targetFramework));
            return this;
        }

        public NuGetOptionals Owners(string owners)
        {
            _parent._owners = owners;
            return this;
        }

        public NuGetOptionals ProjectUrl(string projectUrl)
        {
            _parent._projectUrl = projectUrl;
            return this;
        }

        public NuGetOptionals IconUrl(string iconUrl)
        {
            _parent._iconUrl = iconUrl;
            return this;
        }

        public NuGetOptionals RequireLicenseAcceptance
        {
            get
            {
                _parent._requireLicenseAcceptance = true;
                return this;
            }
        }

        public NuGetOptionals Tags(string tags)
        {
            _parent._tags = tags;
            return this;
        }

        public NuGetOptionals PathToNuGetExecutable(string path)
        {
            _parent._pathToNuGetExecutable = path;
            return this;
        }

    }
}