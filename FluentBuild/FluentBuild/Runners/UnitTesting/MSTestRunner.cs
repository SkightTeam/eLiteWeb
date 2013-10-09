using System;
using System.IO;
using System.Linq;
using FluentBuild.Utilities;
using File = FluentFs.Core.File;

namespace FluentBuild.Runners.UnitTesting
{
    public class MSTestRunner : FailableInternalExecutable<MSTestRunner>, IAdditionalArguments<MSTestRunner>
    {
        internal readonly ArgumentBuilder _argumentBuilder;
        private readonly IExecutable _executable;
        protected internal string pathToConsoleRunner;
        protected internal string resultsFile;
        protected internal string runConfig;
        protected internal string[] test;
        protected internal string testContainer;
        protected internal string[] testList;
        protected internal string testMetadata;
        protected internal bool unique;
        protected internal string workingDirectory;

        public MSTestRunner(IExecutable executable)
        {
            _executable = executable;
            _argumentBuilder = new ArgumentBuilder("/", ":");
        }

        public MSTestRunner() : this(new Executable())
        {
        }

        #region IAdditionalArguments<MSTestRunner> Members

        public MSTestRunner AddArgument(string name)
        {
            _argumentBuilder.AddArgument(name);
            return this;
        }

        public MSTestRunner AddArgument(string name, string value)
        {
            _argumentBuilder.AddArgument(name, value);
            return this;
        }

        #endregion

        public MSTestRunner WorkingDirectory(string path)
        {
            workingDirectory = path;
            return this;
        }

        public MSTestRunner WorkingDirectory(File path)
        {
            return WorkingDirectory(path.ToString());
        }

        public MSTestRunner TestContainer(string path)
        {
            testContainer = path;
            return this;
        }

        public MSTestRunner TestContainer(File path)
        {
            return TestContainer(path.ToString());
        }

        public MSTestRunner TestMetadata(string path)
        {
            testMetadata = path;
            return this;
        }

        public MSTestRunner TestMetadata(File path)
        {
            return TestMetadata(path.ToString());
        }

        public MSTestRunner Test(params string[] testNames)
        {
            test = testNames;
            return this;
        }

        public MSTestRunner TestList(params string[] testLists)
        {
            testList = testLists;
            return this;
        }

        public MSTestRunner RunConfig(string path)
        {
            runConfig = path;
            return this;
        }

        public MSTestRunner RunConfig(File path)
        {
            return RunConfig(path.ToString());
        }

        public MSTestRunner ResultsFile(string path)
        {
            resultsFile = path;
            return this;
        }

        public MSTestRunner ResultsFile(File path)
        {
            return ResultsFile(path.ToString());
        }

        public MSTestRunner Unique()
        {
            unique = true;
            return this;
        }

        public MSTestRunner PathToConsoleRunner(string path)
        {
            pathToConsoleRunner = path;
            return this;
        }

        public MSTestRunner PathToConsoleRunner(File path)
        {
            pathToConsoleRunner = path.ToString();
            return this;
        }

        internal void AddArgsFromArray(string[] items, string prefix)
        {
            if (items == null || !items.Any())
                return;

            foreach (string arg in items)
            {
                _argumentBuilder.AddArgument(prefix, arg);
            }
        }

        internal void BuildArgs()
        {
            if (!string.IsNullOrEmpty(testContainer))
                _argumentBuilder.AddArgument("testcontainer", testContainer);

            if (!string.IsNullOrEmpty(testMetadata))
                _argumentBuilder.AddArgument("testmetadata", testMetadata);


            AddArgsFromArray(testList, "testList");
            AddArgsFromArray(test, "test");

            if (!string.IsNullOrEmpty(runConfig))
                _argumentBuilder.AddArgument("runconfig", runConfig);

            if (!string.IsNullOrEmpty(resultsFile))
                _argumentBuilder.AddArgument("resultsfile", resultsFile);

            if (unique)
                _argumentBuilder.AddArgument("unique");

            _argumentBuilder.AddArgument("nologo");
        }


        internal override void InternalExecute()
        {
            if (String.IsNullOrEmpty(pathToConsoleRunner))
                throw new FileNotFoundException("Could not automatically find mstest.exe. Please specify it manually using PathToConsoleRunner");

            BuildArgs();
            IExecutable executable = _executable.ExecutablePath(pathToConsoleRunner).UseArgumentBuilder(_argumentBuilder);
            if (!String.IsNullOrEmpty(workingDirectory))
                executable = executable.InWorkingDirectory(workingDirectory);

            //don't throw any errors
            //.WithMessageProcessor()
            int returnCode = executable.Execute();
            //if it returned non-zero then just exit (as a test failed)
            if (returnCode != 0 && base.OnError == OnError.Fail)
            {
                BuildFile.SetErrorState();
                Defaults.Logger.WriteError("ERROR", "MSTest returned non-zero error code");
            }
        }
    }
}