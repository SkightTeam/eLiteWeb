using System;
using System.Text;

namespace FluentBuild.Utilities
{
    ///<summary>
    /// Thrown when the request Windows SDK can not be found
    ///</summary>
    public class SdkNotFoundException : ApplicationException
    {
        private readonly string _message;

        public override string Message
        {
           
            get { return _message; }
        }

        ///<summary>
        /// Creates a new exception and populates the message.
        ///</summary>
        ///<param name="pathsSearched">Paths searched to find the SDK</param>
        public SdkNotFoundException(string pathsSearched)
        {
            var sb = new StringBuilder();
            sb.Append("Could not find the SDK by searching paths ");
            sb.Append(pathsSearched);
            sb.Append(". Please make sure it is installed.");
            _message = sb.ToString();
        }
    }
}