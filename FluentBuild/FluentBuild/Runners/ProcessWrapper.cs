using System;
using System.Diagnostics;

namespace FluentBuild.Runners
{

    internal interface IProcessWrapper : IDisposable
    {
        bool Start();
        void BeginOutputAndErrorReadLine();
        bool WaitForExit(int? milliseconds);
        void Kill();
        int ExitCode { get; }
    }


    internal class ProcessWrapper:IProcessWrapper
    {
        private readonly Process _process;

        public ProcessWrapper(Process process)
        {
            _process = process;
        }

        public bool Start()
        {
            return _process.Start();
        }

        public void BeginOutputAndErrorReadLine()
        {
            _process.BeginOutputReadLine();
            _process.BeginErrorReadLine();
        }

        public bool WaitForExit(int? milliseconds)
        {
			if( milliseconds == null )
			{
				_process.WaitForExit();
				return true;
			}
            return _process.WaitForExit(milliseconds.Value);
        }

        public void Kill()
        {
            _process.Kill();
        }

        public int ExitCode
        {
            get { return _process.ExitCode; }
        }

        public void Dispose()
        {
            _process.Dispose();
        }
    }
}

