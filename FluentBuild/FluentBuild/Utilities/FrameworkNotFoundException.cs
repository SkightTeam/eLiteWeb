using System;
using System.Text;

namespace FluentBuild.Utilities
{
    ///<summary>
    /// Raises when the desired .NET framework can not be found
    ///</summary>
    public class FrameworkNotFoundException : Exception
    {
        private readonly string _message;

        public override string Message
        {
            get { return _message; }
        }

        ///<summary>
        /// Creates a new exception with the message populated
        ///</summary>
        ///<param name="frameworkInstallRoot">Paths that were searched to find the install path</param>
        public FrameworkNotFoundException(string frameworkInstallRoot)
        {
            var sb = new StringBuilder();
            sb.Append("Could not find the .NET Framework install path by searching paths ");
            sb.Append(frameworkInstallRoot);
            sb.Append(". Please make sure it is installed.");
            _message = sb.ToString();
        }
    }
}