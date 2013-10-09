using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;

using FluentBuild.MessageLoggers.MessageProcessing;
using FluentBuild.Utilities;

namespace FluentBuild.Runners.UnitTesting
{
    public interface INUnitRunner : IAdditionalArguments<INUnitRunner>
    {
        ///<summary>
        /// Sets the working directory
        ///</summary>
        ///<param name="path">The working directory for nunit-console</param>
        ///<returns></returns>
        NUnitRunner WorkingDirectory(string path);

        ///<summary>
        /// The assembly to run nunit against
        ///</summary>
        ///<param name="path">path to the assembly</param>
        ///<returns></returns>
        NUnitRunner FileToTest(string path);

        ///<summary>
        /// The assembly to run nunit against
        ///</summary>
        ///<param name="File">build artifact that represents the path to the assembly to test</param>
        ///<returns></returns>
        NUnitRunner FileToTest(FluentFs.Core.File buildArtifact);

        /// <summary>
        /// Manually sets the path to nunit-console.exe. If this is not set then the runner will try and find the file on its own by searching the current folder and its subfolders.
        /// </summary>
        /// <param name="path">Path to nunit-console.exe</param>
        /// <returns></returns>
        NUnitRunner PathToNunitConsoleRunner(string path);

        ///<summary>
        /// Allows you to set the output to be an xml file.
        ///</summary>
        ///<param name="path">path for the output</param>
        ///<returns></returns>
        NUnitRunner XmlOutputTo(string path);

        ///<summary>
        /// Adds a parameter for nunit
        ///</summary>
        ///<param name="name">The name of the parameter</param>
        ///<param name="value">The value of the parameter</param>
        ///<returns></returns>
        NUnitRunner AddParameter(string name, string value);

        ///<summary>
        /// Adds a named parameter to nunit
        ///</summary>
        ///<param name="name">The name of the parameter</param>
        ///<returns></returns>
        NUnitRunner AddParameter(string name);

        NUnitRunner FailOnError { get; }
        NUnitRunner ContinueOnError { get; }
    }

    ///<summary>
    /// Runs nunit against an assembly
    ///</summary>
    public class NUnitRunner : FailableInternalExecutable<NUnitRunner>, INUnitRunner
    {
        internal string _fileToTest;
        internal string _pathToConsoleRunner;
        internal string _workingDirectory;
        private IExecutable _executable;
        private readonly IFileSystemHelper _fileSystemHelper;
        internal ArgumentBuilder _argumentBuilder;

        internal NUnitRunner(IExecutable executable, IFileSystemHelper fileSystemHelper)
        {
            _executable = executable;
            _fileSystemHelper = fileSystemHelper;
            _argumentBuilder = new ArgumentBuilder("/", ":");
        }

        public NUnitRunner() : this (new Executable(), new FileSystemHelper())
        {

        }

        ///<summary>
        /// Sets the working directory
        ///</summary>
        ///<param name="path">The working directory for nunit-console</param>
        ///<returns></returns>
        public NUnitRunner WorkingDirectory(string path)
        {
            _workingDirectory = path;
            return this;
        }

        ///<summary>
        /// The assembly to run nunit against
        ///</summary>
        ///<param name="path">path to the assembly</param>
        ///<returns></returns>
        public NUnitRunner FileToTest(string path)
        {
            _fileToTest = path;
            return this;
        }

        ///<summary>
        /// The assembly to run nunit against
        ///</summary>
        ///<param name="File">build artifact that represents the path to the assembly to test</param>
        ///<returns></returns>
        public NUnitRunner FileToTest(FluentFs.Core.File buildArtifact)
        {
            return FileToTest(buildArtifact.ToString());
        }


        /// <summary>
        /// Manually sets the path to nunit-console.exe. If this is not set then the runner will try and find the file on its own by searching the current folder and its subfolders.
        /// </summary>
        /// <param name="path">Path to nunit-console.exe</param>
        /// <returns></returns>
        public NUnitRunner PathToNunitConsoleRunner(string path)
        {
            _pathToConsoleRunner = path;
            return this;
        }

        ///<summary>
        /// Allows you to set the output to be an xml file.
        ///</summary>
        ///<param name="path">path for the output</param>
        ///<returns></returns>
        public NUnitRunner XmlOutputTo(string path)
        {
            return AddParameter("xml", path);
        }

        ///<summary>
        /// Adds a parameter for nunit
        ///</summary>
        ///<param name="name">The name of the parameter</param>
        ///<param name="value">The value of the parameter</param>
        ///<returns></returns>
        public NUnitRunner AddParameter(string name, string value)
        {
            _argumentBuilder.AddArgument(name,value);
            return this;
        }

        ///<summary>
        /// Adds a named parameter to nunit
        ///</summary>
        ///<param name="name">The name of the parameter</param>
        ///<returns></returns>
        public NUnitRunner AddParameter(string name)
        {
            _argumentBuilder.AddArgument(name);
            return this;
        }


        internal void BuildArgs()
        {
            _argumentBuilder.StartOfEntireArgumentString = _fileToTest;
            _argumentBuilder.AddArgument("nologo");
            _argumentBuilder.AddArgument("nodots");
            _argumentBuilder.AddArgument("xmlconsole");
        }

        ///<summary>
        /// Attempts to find and then run nunit-console
        ///</summary>
        ///<exception cref="FileNotFoundException">Occurs when nunit-console.exe can not be found</exception>
        [Obsolete("This is replaced by Tasks.Run.UnitTestFramework(args)", false)]
        public void Execute()
        {
            InternalExecute();    
        }

        internal override void InternalExecute()
        {
            if (String.IsNullOrEmpty(_pathToConsoleRunner))
            {
              _pathToConsoleRunner = _fileSystemHelper.Find("nunit-console.exe");
                if (_pathToConsoleRunner == null)
                    throw new FileNotFoundException("Could not automatically find nunit-console.exe. Please specify it manually using NunitRunner.PathToNunitConsoleRunner");
            }

            var executable = _executable.ExecutablePath(_pathToConsoleRunner);
            BuildArgs();
            executable = executable.UseArgumentBuilder(_argumentBuilder);
            executable = executable.SucceedOnNonZeroErrorCodes();

            //var executable = executablePath.SucceedOnNonZeroErrorCodes();
            //if (OnError == OnError.Fail)
            //    executable = executable.FailOnError;
            //else if (OnError == OnError.Continue)
            //    executable = executable.ContinueOnError;

            if (!String.IsNullOrEmpty(_workingDirectory))
                executable = executable.InWorkingDirectory(_workingDirectory);
            
            //don't throw an errors
            var returnCode = executable.WithMessageProcessor(new NunitMessageProcessor()).Execute();
            //if it returned non-zero then just exit (as a test failed)
            if (returnCode != 0 && base.OnError == OnError.Fail)
            {
                BuildFile.SetErrorState();
                Defaults.Logger.WriteError("ERROR", "Nunit returned non-zero error code");
            }
        }

        public INUnitRunner AddArgument(string name)
        {
            _argumentBuilder.AddArgument(name);
            return this;
        }

        public INUnitRunner AddArgument(string name, string value)
        {
            _argumentBuilder.AddArgument(name,value);
            return this;
        }
    }
}