using System.Collections.Generic;
using System.Collections.Specialized;

namespace FluentBuild.Utilities
{
    ///<summary>
    /// Used to find a value in the registry
    ///</summary>
    public interface IRegistryKeyValueFinder
    {
        ///<summary>
        /// Finds the first value in a string of paths to search
        ///</summary>
        ///<param name="keysToCheck">A list of keys to check</param>
        ///<returns>The name value pair of the key (name is the key and value is the value of that key) or null if no key could be found</returns>
        KeyValuePair<string, string> FindFirstValue(params string[] keysToCheck);
    }

    ///<summary>
    /// Searches the registry for keys.
    ///</summary>
    public class RegistryKeyValueFinder : IRegistryKeyValueFinder
    {
        private readonly IRegistryWrapper _registryWrapper;

        internal RegistryKeyValueFinder(IRegistryWrapper registryWrapper)
        {
            _registryWrapper = registryWrapper;
        }

        public RegistryKeyValueFinder() : this(new RegistryWrapper())
        {
        }

        #region IRegistryKeyValueFinder Members

        public KeyValuePair<string, string> FindFirstValue(params string[] keysToCheck)
        {
            foreach (string keyToCheck in keysToCheck)
            {
                string[] parts = keyToCheck.Split(@"\".ToCharArray());
                IRegistryKeyWrapper key = _registryWrapper.OpenLocalMachineKey(parts[0]);
                for (int i = 1; i < parts.Length - 1; i++)
                {
                    key = key.OpenSubKey(parts[i]);
                    if (key == null) //key is null if it does not exist
                        break;
                }

                if (key != null) //could open all keys now try to get the value
                    return new KeyValuePair<string, string>(key.Name, key.GetValue(parts[parts.Length - 1]).ToString());
            }
            return new KeyValuePair<string, string>(); //can't find anything so return an emtpy string
        }

        #endregion
    }
}