using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentBuild;
using FluentBuild.Utilities;
using FluentFs.Core;
using OnError = FluentFs.Core.OnError;

namespace Build.Samples
{
    public class MultiTargetingSample : BuildFile
    {
        private Directory _baseDirectory;
        private Directory _compileDirectory;
        private File _outputAssembly;


        public MultiTargetingSample()
        {
            _baseDirectory = new Directory(Properties.CurrentDirectory);
            _compileDirectory = _baseDirectory.SubFolder("compile");
            _outputAssembly = _compileDirectory.File("output.dll");

            AddTask(Clean);
            AddTask("Compile .NET 2.0", Compile_20);
            AddTask(Clean);
            AddTask("Compile .NET 3.0", Compile_30);
        }

        public void Clean()
        {
            _compileDirectory.Delete(OnError.Continue).Create();
        }

        private void Compile_20()
        {
            Defaults.FrameworkVersion = FrameworkVersion.NET2_0;
            Compile();
            _outputAssembly.Copy.To(@"c:\Releases\NET20\");
        }

        private void Compile_30()
        {
            Defaults.FrameworkVersion = FrameworkVersion.NET3_0;
            Compile();
            _outputAssembly.Copy.To(@"c:\Releases\NET30\");
        }


        private void Compile()
        {
            var sources = _baseDirectory.SubFolder("src").Files();
            Task.Build.Csc.Target.Library(csc=>csc.AddSources(sources).OutputFileTo(_outputAssembly));
        }
    }
}
