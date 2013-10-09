using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentBuild;
using FluentFs.Core;

namespace Build.Samples
{
    public class DebugBuild : BuildFile
    {
        protected Directory _baseDirectory;
        protected Directory _compileDirectory;
        protected File _outputAssembly;
        protected Directory _toolsDirectory;
        protected File _creditCardProcessor;

        public DebugBuild()
        {
            _baseDirectory = new Directory(Properties.CurrentDirectory);
            _compileDirectory = _baseDirectory.SubFolder("compile");
            _toolsDirectory = _baseDirectory.SubFolder("tools");

            _outputAssembly = _compileDirectory.File("output.dll");
            _creditCardProcessor = _toolsDirectory.File("CreditCardDevelopment.dll");

            AddTask(Clean);
            AddTask(Compile);
        }

        protected virtual void Compile()
        {
            var sources = _baseDirectory.SubFolder("src").Files();
            Task.Build.Csc.Target.Library(csc => csc.AddSources(sources).IncludeDebugSymbols.OutputFileTo(_outputAssembly).AddRefences(_creditCardProcessor));
        }

        protected void Clean()
        {
            _compileDirectory.Delete(OnError.Continue).Create();
        }
    }

    public class PublishBuild : DebugBuild
    {
        public PublishBuild()
        {
            _creditCardProcessor = _toolsDirectory.File("CreditCardProduction.dll");
        }

        protected override void Compile()
        {
            var sources = _baseDirectory.SubFolder("src").Files();
            Task.Build.Csc.Target.Library(csc => csc.AddSources(sources).OutputFileTo(_outputAssembly).AddRefences(_creditCardProcessor));
        }
    }
}
