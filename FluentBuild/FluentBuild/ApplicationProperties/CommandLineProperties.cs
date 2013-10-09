using System.Collections.Specialized;
using System.ComponentModel;

namespace FluentBuild.ApplicationProperties
{
    ///<summary>
    /// Gets items around the command line properties
    ///</summary>
    public interface ICommandLineProperties
    {
        ///<summary>
        /// Gets a property from the command line /p:name=value
        ///</summary>
        ///<param name="name">find the property by its name</param>
        ///<returns>value if present</returns>
        string GetProperty(string name);

        ///<summary>
        /// Adds a property to the internal property collection
        ///</summary>
        ///<param name="name">The name of the property</param>
        ///<param name="value">The value of the property</param>
        void Add(string name, string value);
    }

    ///<summary>
    /// Accesses properties passed in via the command line
    ///</summary>
    public class CommandLineProperties : ICommandLineProperties
    {
        private static readonly NameValueCollection _properties = new NameValueCollection();

        internal CommandLineProperties()
        {
        }

        internal NameValueCollection Properties
        {
            get { return _properties; }
        }

        ///<summary>
        /// Gets a property from the command line /p:name=value
        ///</summary>
        ///<param name="name">find the property by its name</param>
        ///<returns>value if present</returns>
        public string GetProperty(string name)
        {
            return _properties[name];
        }


        ///<summary>
        /// Adds a property to the internal property collection
        ///</summary>
        ///<param name="name">The name of the property</param>
        ///<param name="value">The value of the property</param>
        public void Add(string name, string value)
        {
            _properties.Add(name, value);
        }
    }
}