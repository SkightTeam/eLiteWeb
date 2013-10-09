using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading;

using FluentBuild.MessageLoggers.MessageProcessing;
using FluentBuild.Utilities;
using System.Linq;
using FluentFs.Core;
using OnError = FluentBuild.Utilities.OnError;

namespace FluentBuild.Runners
{
    ///<summary>
    /// Represents an executable to be run
    ///</summary>
    public interface IExecutable : IFailable<IExecutable>, IAdditionalArguments<IExecutable>
    {
        ///<summary>
        /// Sets the arguments to pass to the executable
        ///</summary>
        ///<param name="arguments">The arguments to pass</param>
        [Obsolete("Replaced with AddArgument", true)]
        IExecutable WithArguments(params string[] arguments);

        ///<summary>
        /// Sets the working directory
        ///</summary>
        ///<param name="directory">path to the working directory</param>
        IExecutable InWorkingDirectory(string directory);

        ///<summary>
        /// Sets the working directory
        ///</summary>
        ///<param name="directory">path to the working directory</param>
        IExecutable InWorkingDirectory(FluentFs.Core.Directory directory);

        ///<summary>
        /// Executes the executable with the provided options.
        ///</summary>
        /// <returns>the return code of the process</returns>
        [Obsolete("Replaced with Task.Run.Executable", false)]
        int Execute();

     

        ///<summary>
        /// Sets the executable to run
        ///</summary>
        ///<param name="path">path to the executable</param>
        IExecutable ExecutablePath(string path);

        /// <summary>
        /// Sets the executable to run
        /// </summary>
        /// <param name="path">path to the executable</param>
        /// <returns></returns>
        IExecutable ExecutablePath(File path);

        ///<summary>
        /// Allows you to override the default message parser. This is typically used for parsing a runners output (i.e. nunit outputs in xml so a different parser is used to transform messages)
        ///</summary>
        ///<param name="processor">The processor to use</param>
        IExecutable WithMessageProcessor(IMessageProcessor processor);

        ///<summary>
        /// Sets the timeout the process should run for.
        ///</summary>
        ///<param name="timeoutInMiliseconds">The timeout value in milliseconds</param>
        IExecutable SetTimeout(int timeoutInMiliseconds);

        ///<summary>
        /// Changes the bahavior of the executeable. If the executable returns a non-zero error code it will still be considered
        /// a success.
        ///</summary>
        IExecutable SucceedOnNonZeroErrorCodes();


        /// <summary>
        /// Allows the consumer to inject the argumentBuild if it was used by a calling runner
        /// This allows the consumer to have it's own internal argumentBuilder and not have to 
        /// loop over the arguments when calling an Executable
        /// </summary>
        /// <param name="argumentBuilder">The builder to inject</param>
        IExecutable UseArgumentBuilder(ArgumentBuilder argumentBuilder);
    }

    ///<summary>
    /// An executable to be run
    ///</summary>
    public class Executable : InternalFailable<IExecutable>, IExecutable
    {
        private readonly IActionExcecutor _actionExcecutor;
        private IMessageProcessor _messageProcessor;
        private readonly object _errorLock;
        private readonly object _outputLock;
        private readonly StringBuilder _error;
        private readonly StringBuilder _output;
        internal string Path;
		//private bool _noErrorMessageWhenExitCodeZero = true;
		private int? _timeoutInMiliSeconds = null;
        internal string WorkingDirectory;

        private bool _succeedOnNonZeroErrorCodes;
        private ArgumentBuilder _argumentBuilder;

        ///<summary>
        /// Instantiates a new executable
        ///</summary>
        public Executable() : this(new DefaultMessageProcessor(), new ActionExcecutor())
        {
            
        }

        internal Executable(IMessageProcessor messageProcessor, IActionExcecutor actionExcecutor)
        {
            _messageProcessor = messageProcessor;
            _errorLock = new object();
            _outputLock = new object();
            _error = new StringBuilder();
            _output = new StringBuilder();
            _actionExcecutor = actionExcecutor;
            _argumentBuilder = new ArgumentBuilder("/", " ");
        }

        ///<summary>
        /// instantiates an executable with the path to the assembly specified
        ///</summary>
        ///<param name="path">Path to the executable to run</param>
        public Executable(string path) : this()
        {
            Path = path;
        }

        #region IExecutable Members

        public IExecutable ExecutablePath(File path)
        {
            return ExecutablePath(path.ToString());
        }

        public IExecutable WithMessageProcessor(IMessageProcessor processor)
        {
            _messageProcessor = processor;
            return this;
        }

        public IExecutable SucceedOnNonZeroErrorCodes()
        {
            _succeedOnNonZeroErrorCodes = true;
            return this;
        }

        public IExecutable UseArgumentBuilder(ArgumentBuilder argumentBuilder)
        {
            _argumentBuilder = argumentBuilder;
            return this;
        }

        [Obsolete("Replaced with AddArgument")]
        public IExecutable WithArguments(params string[] arguments)
        {
            foreach (var argument in arguments)
            {
                _argumentBuilder.AddArgument(argument);
            }
            return this;
        }

        public IExecutable InWorkingDirectory(string directory)
        {
            WorkingDirectory = directory;
            return this;
        }

        public IExecutable InWorkingDirectory(FluentFs.Core.Directory directory)
        {
            WorkingDirectory = directory.ToString();
            return this;
        }

		public IExecutable SetTimeout(int timeoutInMiliseconds)
		{
			_timeoutInMiliSeconds = timeoutInMiliseconds;
			return this;
		}

        [Obsolete("This is replaced with Task.Run.Executable()", true)]
        public int Execute()
        {
            InternalExecute();
            return ExitCode;
        }
        
        internal override void InternalExecute()
        {
            Execute("exec");
        }

        public IExecutable ExecutablePath(string path)
        {
            Path = path;
            return this;
        }

        #endregion

         internal IProcessWrapper CreateProcess()
        {
            var process = new Process();
            process.StartInfo.FileName = Path;
            process.StartInfo.Arguments = _argumentBuilder.Build();

            //redirect to a stream so we can parse it and display it
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.CreateNoWindow = true;
            
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.RedirectStandardError = true;
            process.StartInfo.ErrorDialog = false;

            if (!String.IsNullOrEmpty(WorkingDirectory))
                process.StartInfo.WorkingDirectory = WorkingDirectory;

            process.ErrorDataReceived += ProcessErrorDataReceived;
            process.OutputDataReceived += ProcessOutputDataReceived;

            return new ProcessWrapper(process);
        }

        internal int Execute(string prefix)
        {
            Defaults.Logger.Write(prefix, Path + _argumentBuilder.Build());
            int exitCode = 0;
            using (IProcessWrapper process = CreateProcess())
            {
                try
                {
                    process.Start();
                    process.BeginOutputAndErrorReadLine();
					if (!process.WaitForExit(_timeoutInMiliSeconds))
					{
					    HandleTimeout(process);
					}

//					if( _noErrorMessageWhenExitCodeZero && process.ExitCode == 0)
//					{
//						_output.Append( _error );
//						_error.Clear();
//					}

                    _messageProcessor.Display(prefix, _output.ToString(), _error.ToString(), process.ExitCode);
                    exitCode = process.ExitCode;

                    //if we are supposed to fail on errors
                    //and there was an error
                    //and the calling code has decided not to deal with the error code (by setting SucceedOnNonZeroErrorCodes
                    //then set the environment exit code
                    if (OnError == OnError.Fail && exitCode != 0 && _succeedOnNonZeroErrorCodes == false)
                    {
                        BuildFile.SetErrorState();
                        throw new ApplicationException("Executable returned an error code of " + exitCode);
                    }

                }
                catch (Exception e)
                {
                    if (OnError == OnError.Fail)
                        throw;
                    Debug.Write(prefix, "An error occurred running a process but FailOnError was set to false. Error: " + e);
                }
            }

            this.ExitCode = exitCode;
            return exitCode;
        }

        public int ExitCode { get; set; }

        private void HandleTimeout(IProcessWrapper process)
        {
            Defaults.Logger.WriteDebugMessage("TIMEOUT!");
            process.Kill();
            Thread.Sleep(1000); //wait one second so that the process has time to exit

            if (OnError == OnError.Fail)
            {
                //exit code should only be set if we want the application to fail on error
                BuildFile.SetErrorState(); //set our ExitCode to non-zero so consumers know we errored
            }
        }

        //lock objects in case events fire out of order
        private void ProcessOutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            lock (_outputLock)
                _output.AppendLine(e.Data);
        }

        private void ProcessErrorDataReceived(object sender, DataReceivedEventArgs e)
        {
            lock (_errorLock)
                _error.AppendLine(e.Data);
        }

        public IExecutable AddArgument(string name)
        {
            _argumentBuilder.AddArgument(name);
            return this;
        }

        public IExecutable AddArgument(string name, string value)
        {
            _argumentBuilder.AddArgument(name,value);
            return this;
        }
    }
}