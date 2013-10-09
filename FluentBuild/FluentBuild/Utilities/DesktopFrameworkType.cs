using System;
using FluentBuild.FrameworkFinders;

namespace FluentBuild.Utilities
{
    ///<summary>
    /// Indicates if the type is client or full (used in .NET 4.0 and higher)
    ///</summary>
    public class DesktopFrameworkType
    {
        private readonly IFrameworkFinder _clientFinder;
        private readonly IFrameworkFinder _fullFinder;

    
        public DesktopFrameworkType(IFrameworkFinder clientFinder, IFrameworkFinder fullFinder)
        {
            _clientFinder = clientFinder;
            _fullFinder = fullFinder;
        }

        ///<summary>
        /// Creates a FrameworkVersion object for the client type
        ///</summary>
        public FrameworkVersion Client
        {
            get { return new FrameworkVersion(".NET 4.0 (Client)", _clientFinder); }
        }

        ///<summary>
        /// Creates a FrameworkVersion object for the full type
        ///</summary>
        public FrameworkVersion Full
        {
            get { return new FrameworkVersion(".NET 4.0 (Full)", _fullFinder); }
        }
    }
}