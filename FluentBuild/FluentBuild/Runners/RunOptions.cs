using System;
using FluentBuild.Runners.UnitTesting;
using FluentBuild.Runners.Zip;
using FluentBuild.Utilities;

namespace FluentBuild.Runners
{
    public class RunOptions
    {
        private readonly IActionExcecutor _actionExcecutor;

        public RunOptions(IActionExcecutor actionExcecutor)
        {
            _actionExcecutor = actionExcecutor;
        }

        public RunOptions() : this(new ActionExcecutor())
        {
        }

        public IZipOptions Zip
       
            { get { return new ZipOptions();}}


        public int Executable(Func<Executable, object> args)
        {
            var implementation = new Executable();
            args(implementation);
            implementation.InternalExecute();
            return implementation.ExitCode;
        }

        public int Executable(Action<Executable> args)
        {
            var implementation = new Executable();
            args(implementation);
            implementation.InternalExecute();
            return implementation.ExitCode;
        }

        public void Debugger()
        {
            System.Diagnostics.Debugger.Break();
            System.Diagnostics.Debugger.Launch();
        }

        public void ILMerge(Action<IILMerge> args)
        {
            var implementation = new ILMerge();
            args(implementation);
            implementation.InternalExecute();
        }

        public UnitTestFrameworkArgs UnitTestFramework { get { return new UnitTestFrameworkArgs();  }}
    }
}