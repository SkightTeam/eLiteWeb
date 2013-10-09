using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using FluentBuild.Runners;
using FluentBuild.Utilities;
using Directory = FluentFs.Core.Directory;

namespace FluentBuild.Publishing.NuGet
{
    //http://docs.nuget.org/docs/creating-packages/creating-and-publishing-a-package
    //Mandatory: deployFolder, Id, version, description, authors

    public class NuGetPublisher : InternalExecutable
    {
        private readonly IFileSystemHelper _fileSystemHelper;
        private readonly IExecutable _executable;
        internal string _projectId;
        internal string _version;
        internal string _authors;
        internal string _owners;
        internal Directory _deployFolder;
        internal string _projectUrl;
        internal string _iconUrl;
        internal bool _requireLicenseAcceptance;
        internal string _description;
        internal string _tags;
        internal string _pathToNuGetExecutable;
        internal string _apiKey;
        internal string _title;
        internal string _releaseNotes;
        internal string _summary;
        internal string _language;
        internal string _additionalManifestData;
        internal string _licenseUrl;
        internal string _copyright;
        internal IList<string> _references;
        internal IList<FrameworkAssembly> _frameworkAssemblies;
        public IList<DependencyGroup> _depenencyGroups;

        public NuGetPublisher() : this(new FileSystemHelper(), new Executable())
        {
        }

        internal NuGetPublisher(IFileSystemHelper fileSystemHelper, IExecutable executable)
        {
            _fileSystemHelper = fileSystemHelper;
            _executable = executable;
            _references = new List<string>();
            _frameworkAssemblies = new List<FrameworkAssembly>();
            _depenencyGroups = new List<DependencyGroup>();
        }

        public ProjectIdMandatory DeployFolder(string path)
        {
            return DeployFolder(new Directory(path));
        }

        public ProjectIdMandatory DeployFolder(Directory path)
        {
            _deployFolder = path;
            return new ProjectIdMandatory(this);
        }


        internal string CreateSchema()
        {
            var sb = new StringBuilder();
            sb.AppendLine("<?xml version=\"1.0\"?>");
            sb.AppendLine("<package xmlns=\"http://schemas.microsoft.com/packaging/2010/07/nuspec.xsd\">");
            sb.AppendLine("<metadata>");
            sb.AppendLine("<id>" + _projectId + "</id>");
            sb.AppendLine("<version>" + this._version + "</version>");
            sb.AppendLine("<authors>" + _authors + "</authors>");
            sb.AppendLine("<description>" + _description + "</description>");

            if (!string.IsNullOrEmpty(_owners))
                sb.AppendLine("<owners>" + _owners + "</owners>");
            if (!string.IsNullOrEmpty(_projectUrl))
                sb.AppendLine("<projectUrl>" + _projectUrl + "</projectUrl>");
            if (!string.IsNullOrEmpty(_iconUrl))
                sb.AppendLine("<iconUrl>" + _iconUrl + "</iconUrl>");
            if (!string.IsNullOrEmpty(_tags))
                sb.AppendLine("<tags>" + _tags + "</tags>");
            if (!string.IsNullOrEmpty(_title))
                sb.AppendLine("<title>" + _title + "</title>");
            if (!string.IsNullOrEmpty(_releaseNotes))
                sb.AppendLine("<releaseNotes>" + _releaseNotes + "</releaseNotes>");
            if (!string.IsNullOrEmpty(_summary))
                sb.AppendLine("<summary>" + _summary + "</summary>");
            if (!string.IsNullOrEmpty(_language))
                sb.AppendLine("<language>" + _language + "</language>");
            if (!string.IsNullOrEmpty(_licenseUrl))
                sb.AppendLine("<licenseUrl>" + _licenseUrl + "</licenseUrl>");
            if (!string.IsNullOrEmpty(_copyright))
                sb.AppendLine("<copyright>" + _copyright + "</copyright>");

            if (_references.Count > 0)
            {
                sb.AppendLine("<references>");
                foreach (var reference in _references)
                {
                    sb.AppendFormat("<reference file=\"{0}\" />{1}", reference, Environment.NewLine);
                }
                sb.AppendLine("</references>");
            }

            if (_frameworkAssemblies.Count > 0)
            {
                sb.AppendLine("<frameworkAssemblies>");
                foreach (var reference in _frameworkAssemblies)
                {
                    sb.AppendFormat(reference.ToString());
                }
                sb.AppendLine("</frameworkAssemblies>");
            }

            if (_depenencyGroups.Count > 0)
            {
                sb.AppendLine("<dependencies>");
                foreach (var dependencyGroup in _depenencyGroups)
                {
                    sb.AppendLine(dependencyGroup.ToString());
                }
                sb.AppendLine("</dependencies>");
            }
            

            if (!string.IsNullOrEmpty(_additionalManifestData))
                sb.AppendLine(_additionalManifestData);

            sb.AppendLine("<requireLicenseAcceptance>" + _requireLicenseAcceptance.ToString() + "</requireLicenseAcceptance>");
           
            sb.AppendLine("</metadata>");
            sb.AppendLine("</package>");
            return sb.ToString();
        }


        internal override void InternalExecute()
        {
            if (string.IsNullOrEmpty(_pathToNuGetExecutable))
                _pathToNuGetExecutable = _fileSystemHelper.Find("nuget.exe");
            
            if (string.IsNullOrEmpty(_pathToNuGetExecutable))
                throw new FileNotFoundException("Could not locate nuget.exe. Please specify it manually using PathToNuGetExecutable()");

            var stream = _fileSystemHelper.CreateFile(_deployFolder.File(_projectId + ".nuspec").ToString());
            using (var fs = new StreamWriter(stream))
            {
                fs.Write(CreateSchema());
            }

            //ensure latest version of nuget
            Defaults.Logger.WriteDebugMessage("Updating NuGet to the latest version");
            var ab = new ArgumentBuilder {StartOfEntireArgumentString = "Update -self"};
            _executable.ExecutablePath(_pathToNuGetExecutable).UseArgumentBuilder(ab).Execute();

            //configure the API key
            Defaults.Logger.WriteDebugMessage("Configuring the API Key");
            ab.StartOfEntireArgumentString = "setApiKey " + _apiKey;
            _executable.ExecutablePath(_pathToNuGetExecutable).UseArgumentBuilder(ab).Execute();

            //package it
            Defaults.Logger.WriteDebugMessage("Creating the package");
            ab.StartOfEntireArgumentString = "Pack " + _projectId + ".nuspec";
            var inWorkingDirectory = _executable.ExecutablePath(_pathToNuGetExecutable).UseArgumentBuilder(ab).InWorkingDirectory(_deployFolder);
            inWorkingDirectory.Execute();

            //NuGet Push YourPackage.nupkg
            Defaults.Logger.WriteDebugMessage("publishing the package");
            ab.StartOfEntireArgumentString = "Push " + _projectId + "." + _version + ".nupkg";
            _executable.ExecutablePath(_pathToNuGetExecutable).UseArgumentBuilder(ab).InWorkingDirectory(_deployFolder).Execute();
        }
    }
}
