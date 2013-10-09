using System;
using System.Collections.Generic;
using System.Text;

using FluentBuild.Runners;
using FluentBuild.Utilities;
using FluentFs.Core;

namespace FluentBuild.Compilation
{
    ///<summary>
    /// A task around builds that will execute a compiler to generate an assembly.
    ///</summary>
    public class BuildTask :InternalExecutable, IAdditionalArguments<BuildTask>
    {
        private readonly List<string> _references = new List<string>();
        internal readonly List<Resource> Resources = new List<Resource>();
        private readonly List<string> _sources = new List<string>();
        private readonly IActionExcecutor _actionExcecutor;
        //private readonly Dictionary<string, string> _additionalArguments = new Dictionary<string, string>();  
        internal readonly string Compiler;
        private bool _includeDebugSymbols;
        private string _outputFileLocation;
        private IList<String> _defineSymbols = new List<string>();
        internal ArgumentBuilder _argumentBuilder;

        public BuildTask() : this(new ActionExcecutor(), "", "")
        {
            
        }

        protected internal BuildTask(IActionExcecutor actionExcecutor, string compiler, string targetType)
        {
            _actionExcecutor = actionExcecutor;
            Compiler = compiler;
            TargetType = targetType;
            _argumentBuilder = new ArgumentBuilder("/", ":");
        }

        public BuildTask(string compiler, string targetType) : this(new ActionExcecutor(), compiler, targetType)
        {
        }

        /// <summary>
        /// Set the output file type
        /// </summary>
        //public Target Target { get; set; }

        protected internal string TargetType { get; set; }

        /// <summary>
        /// Adds an aditional argument to be passed to the command line
        /// </summary>
        /// <param name="name">The name of the parameter (with no '/')</param>
        /// <returns></returns>
        public BuildTask AddArgument(string name)
        {
            _argumentBuilder.AddArgument(name);
            return this;
        }

        /// <summary>
        /// Adds an aditional argument to be passed to the command line
        /// </summary>
        /// <param name="name">The name of the parameter (with no '/')</param>
        /// <param name="value">The value of the parameter</param>
        /// <returns></returns>
        public BuildTask AddArgument(string name, string value)
        {
            _argumentBuilder.AddArgument(name,value);
            return this;
        }

        /// <summary>
        /// Sets if Debug Symbols are generated. Defaults to False.
        /// </summary>
        public BuildTask IncludeDebugSymbols
        {
            get
            {
                _includeDebugSymbols = true;
                return this;
            }
        }

        /// <summary>
        /// Adds a compilation symbol
        /// </summary>
        /// <param name="symbol">The symbol to include</param>
        /// <returns></returns>
        public BuildTask DefineSymbol(string symbol)
        {
            _defineSymbols.Add(symbol);
            return this;

        }

        internal void BuildArgs()
        {

                _argumentBuilder.AddQuotedArgument("out", _outputFileLocation);

                foreach (Resource res in Resources)
                {
                    //res.ToString() does the work of converting the resource to a string
                    _argumentBuilder.AddArgument("resource", res.ToString());
                }

                _argumentBuilder.AddArgument("target", TargetType);

                foreach (string reference in _references)
                {
                    _argumentBuilder.AddQuotedArgument("reference", reference);
                }

                //var resources = new StringBuilder();

                foreach (var defineSymbol in _defineSymbols)
                {
                    _argumentBuilder.AddArgument("define", defineSymbol);
                }


                

                //args += String.Format("/out:\"{0}\" {1} /target:{2} {3} {4}", _outputFileLocation, resources, TargetType, references, sources);
                if (_includeDebugSymbols)
                    _argumentBuilder.AddArgument("debug");



                var sources = new StringBuilder();
                foreach (string source in _sources)
                {
                    sources.Append(" \"" + source + "\"");
                }

                _argumentBuilder.EndOfEntireArgumentString = sources.ToString();
         
        }

        /// <summary>
        /// Sets the output file location
        /// </summary>
        /// <param name="outputFileLocation">The path to output the file to</param>
        /// <returns></returns>
        public BuildTask OutputFileTo(string outputFileLocation)
        {
            _outputFileLocation = outputFileLocation;
            return this;
        }

        /// <summary>
        /// Sets the output file location
        /// </summary>
        /// <param name="artifact">The BuildArtifact to output the file to</param>
        /// <returns></returns>
        public BuildTask OutputFileTo(File artifact)
        {
            return OutputFileTo(artifact.ToString());
        }

        /// <summary>
        /// Adds a reference to be included in the build
        /// </summary>
        /// <param name="fileNames">a param array of string paths to the reference</param>
        /// <returns></returns>
        public BuildTask AddRefences(params string[] fileNames)
        {
            _references.AddRange(fileNames);
            return this;
        }

        /// <summary>
        /// Adds a reference to be included in the build
        /// </summary>
        /// <param name="artifacts">a param array of BuildArtifacts to the reference</param>
        /// <returns></returns>
        public BuildTask AddRefences(params File[] artifacts)
        {
            foreach (var buildArtifact in artifacts)
            {
                _references.Add(buildArtifact.ToString());
            }
            return this;
        }

        /// <summary>
        /// Adds a single resource to be included in the build
        /// </summary>
        /// <param name="fileName">a resource file to include</param>
        /// <returns></returns>
        public BuildTask AddResource(string fileName)
        {
            AddResource(fileName, null);
            return this;
        }
        
        /// <summary>
        /// Adds a single resource to be included in the build
        /// </summary>
        /// <param name="fileName">a resource file to include</param>
        /// <param name="identifier">the identifier that code uses to refer to the resource</param>
        /// <returns></returns>
        public BuildTask AddResource(string fileName, string identifier)
        {
            Resources.Add(new Resource(fileName, identifier));
            return this;
        }

        ///<summary>
        /// Adds a fileset of resources to be included in the build
        ///</summary>
        ///<param name="fileSet">The fileset containing the resouces</param>
        ///<returns></returns>
        public BuildTask AddResources(FileSet fileSet)
        {
            foreach (string file in fileSet.Files)
            {
                Resources.Add(new Resource(file));
            }
            return this;
        }

        

        ///<summary>
        /// Executes the compliation with the provided parameters
        ///</summary>
        [Obsolete("This has been replaced with Task.Build(Using.[Compiler]). This method will dissapear in future versions.", false)]
        public void Execute()
        {
            InternalExecute();
        }

        internal override void InternalExecute()
        {
            BuildArgs();
            string compilerWithoutExtentions = Compiler.Substring(0, Compiler.IndexOf("."));
            Defaults.Logger.Write(compilerWithoutExtentions, String.Format("Compiling {0} files to '{1}'", _sources.Count, _outputFileLocation));
            var pathToCompiler = Defaults.FrameworkVersion.GetPathToFrameworkInstall() + "\\" + Compiler;
            Defaults.Logger.WriteDebugMessage("Compile Using: " + pathToCompiler+ " " + _argumentBuilder.Build());
            _actionExcecutor.Execute((Action<Executable>) (x => x.ExecutablePath(pathToCompiler).UseArgumentBuilder(_argumentBuilder)));
            Defaults.Logger.WriteDebugMessage("Done Compiling");
        }

        ///<summary>
        ///Adds in the source files to compile. This method is additive. It can be called multiple times without issue.
        ///</summary>
        ///<param name="fileset">A FileSet containing the files to be compiled.</param>
        ///<returns></returns>
        public BuildTask AddSources(FileSet fileset)
        {
            _sources.AddRange(fileset.Files);
            return this;
        }
    }
}