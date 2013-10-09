using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;

namespace FluentBuild.Utilities
{
    public interface IAdditionalArguments<out T>
    {
        /// <summary>
        /// Adds an aditional argument to be passed to the command line
        /// </summary>
        /// <param name="name">The name of the parameter (with no prefixing character)</param>
        /// <returns></returns>
        T AddArgument(string name);

        /// <summary>
        /// Adds an aditional argument to be passed to the command line
        /// </summary>
        /// <param name="name">The name of the parameter (with no prefixing character)</param>
        /// <param name="value">The value of the parameter</param>
        /// <returns></returns>
        T AddArgument(string name, string value);
    }

    public class ArgumentBuilder 
    {
        //internal readonly Dictionary<string, string> _additionalArguments = new Dictionary<string, string>();
        internal readonly IList<KeyValuePair<string,string>> _additionalArguments  = new List<KeyValuePair<string, string>>();
        
        public ArgumentBuilder()
        {
            Prefix = "/";
            Seperator = ":";
            StartOfEntireArgumentString = "";
        }

        public ArgumentBuilder(string prefix, string seperator)
        {
            Prefix = prefix;
            Seperator = seperator;
        }

        public void AddArgument(string name)
        {
            AddArgument(name, null);
        }

        public void AddArgument(string name, string value)
        {
            _additionalArguments.Add(new KeyValuePair<string, string>(name,value));
        }

        public void AddQuotedArgument(string name, string value)
        {
            AddArgument(name, "\"" + value +"\"");
        }

        public string Prefix { get; set; }
        public string Seperator { get; set; }

        public string StartOfEntireArgumentString { get; set; }
        public string EndOfEntireArgumentString { get; set; }

        public string Build()
        {
            
            var args = StartOfEntireArgumentString;
            foreach (var pair in _additionalArguments)
            {
                if (pair.Value == null)
                {
                    args += " " + Prefix + pair.Key;
                }
                else
                {
                    args += " " + Prefix + pair.Key + Seperator +  pair.Value;
                }
            }
            return  (args + EndOfEntireArgumentString).Trim();
        }

        public string FindByName(string name)
        {
            foreach (var argument in _additionalArguments)
            {
                if (argument.Key == name)
                    return argument.Value;
            }
            return null;
        }
    }
}
