using System;

namespace FluentBuild.Runners
{
    ///<summary>
    /// Occurs when an EXE returns a non zero exception
    ///</summary>
    public class ExecutableFailedException : Exception
    {
        ///<summary>
        /// Creates an exception
        ///</summary>
        ///<param name="message">message text to include in the error</param>
        public ExecutableFailedException(string message) : base(message)
        {
            
        }


    }
}