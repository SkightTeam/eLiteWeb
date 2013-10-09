using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using FluentBuild.Runners;
using FluentBuild.Utilities;
using FluentFs.Core;

namespace FluentBuild.Compilation
{
    ///<summary>
    /// Executes MsBuild to create an assembly (or multiple assemblies)
    ///</summary>
    public class MsBuildTask : InternalExecutable, IAdditionalArguments<MsBuildTask>
    {
        private readonly IActionExcecutor _actionExcecutor;
        internal string _projectOrSolutionFilePath;
        private readonly IExecutable _executable;
        internal readonly NameValueCollection Properties;
        internal readonly IList<string> Targets;
        internal string ConfigurationToUse;
        internal string Outdir;

        internal ArgumentBuilder _argumentBuilder;


        internal MsBuildTask(IActionExcecutor actionExcecutor)
        {
            _actionExcecutor = actionExcecutor;
            Targets = new List<string>();
            Properties = new NameValueCollection();
		
            _argumentBuilder = new ArgumentBuilder("/", "=");
        }

        public MsBuildTask() : this(new ActionExcecutor())
        {
        }

        public MsBuildTask ProjectOrSolutionFilePath(string path)
        {
            _projectOrSolutionFilePath = path;
            return this;
        }

        public MsBuildTask ProjectOrSolutionFilePath(File path)
        {
            return ProjectOrSolutionFilePath(path.ToString());
        }

        ///<summary>
        /// Adds a target to run
        ///</summary>
        ///<param name="target">A target that exists in your msbuild file</param>
        ///<returns></returns>
        public MsBuildTask AddTarget(string target)
        {
            Targets.Add(target);
            return this;
        }

        /// <summary>
        /// Sets a property that is passed to msbuild.exe
        /// </summary>
        /// <param name="name">the name of the property to set</param>
        /// <param name="value">the value of the property</param>
        /// <returns></returns>
        public MsBuildTask SetProperty(string name, string value)
        {
            Properties.Add(name, value);
            return this;
        }

        
        
        ///<summary>
        /// Sets the output directory for the msbuild task
        ///</summary>
        /// <remarks>Sets the OutDir property (i.e. /p:OutDir)</remarks>
        ///<param name="path">the output folder</param>
        ///<returns></returns>
        public MsBuildTask OutputDirectory(string path)
        {
            Outdir = path;
            return this;
        }

        ///<summary>
        /// Sets the output directory for the msbuild task
        ///</summary>
        /// <remarks>Sets the OutDir property (i.e. /p:OutDir)</remarks>
        ///<param name="path">the output folder</param>
        ///<returns></returns>
        public MsBuildTask OutputDirectory(FluentFs.Core.Directory path)
        {
            OutputDirectory(path.ToString());
            return this;
        }

        [Obsolete("Replaced by AddArgument")]
		public MsBuildTask WithArguments(string arg)
		{
            _argumentBuilder.AddArgument(arg);
			return this;
		}        
        
        ///<summary>
        /// Sets the configuration to use for the msbuild task
        ///</summary>
        /// <remarks>Sets the configuration property (i.e. /p:Configuration)</remarks>
        ///<param name="configuration">The configuration to use (e.g. Debug, Release, Custom)</param>
        ///<returns></returns>
        public MsBuildTask Configuration(string configuration)
        {
            //actually a property. Just making it a bit easier to consume common items
            ConfigurationToUse = configuration;
            return this;
        }


        internal void AddSetFieldsToArgumentBuilder()
        {
            _argumentBuilder.StartOfEntireArgumentString = _projectOrSolutionFilePath;

            if (!String.IsNullOrEmpty(Outdir))
            {
                //output dir must have trailing slash. Might as well do it for the user
                var tempDir = Outdir;
                if (!tempDir.Trim().EndsWith("\\"))
                    tempDir += "\\";

                _argumentBuilder.AddArgument("p:OutDir", tempDir);
            }

            if (!String.IsNullOrEmpty(ConfigurationToUse))
                _argumentBuilder.AddArgument("p:Configuration", ConfigurationToUse);

            foreach (var propertyName in Properties.Keys)
            {
                _argumentBuilder.AddArgument("p:" + propertyName, Properties.GetValues(propertyName.ToString())[0]);
            }

            foreach (var target in Targets)
            {
                _argumentBuilder.AddArgument("target:", target);
            }
        }


        internal override void InternalExecute()
        {
            if (String.IsNullOrEmpty(_projectOrSolutionFilePath))
                throw new ArgumentException("ProjectOrSolutionFilePath was not set");

            string pathToMsBuild = Defaults.FrameworkVersion.GetPathToFrameworkInstall() + "\\MsBuild.exe";
            AddSetFieldsToArgumentBuilder(); //update the arg builder with all the fields
            _actionExcecutor.Execute<Executable>(x => x.ExecutablePath(pathToMsBuild).UseArgumentBuilder(_argumentBuilder));
        }

        ///<summary>
        /// Executes MSBuild with the provided parameters
        ///</summary>
        [Obsolete("Replaced with Task.Build(Using.MsBuild)", true)]
        public void Execute()
        {
            InternalExecute();
        }

        public MsBuildTask AddArgument(string name)
        {
            _argumentBuilder.AddArgument(name);
            return this;
        }

        public MsBuildTask AddArgument(string name, string value)
        {
            _argumentBuilder.AddArgument(name,value);
            return this;
        }
    }
}

