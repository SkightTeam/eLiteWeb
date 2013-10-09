using System;
using System.Collections.Generic;
using Microsoft.Win32;

namespace FluentBuild.Utilities
{
    internal interface IRegistryKeyWrapper
    {
        IEnumerable<string> GetSubKeyNames();
        IRegistryKeyWrapper OpenSubKey(string keyName);
        object GetValue(string name);
        void Close();
        string Name { get; }
    }

    internal class RegistryKeyWrapper : IRegistryKeyWrapper
    {
        private readonly RegistryKey _key;

        public RegistryKeyWrapper(RegistryKey key)
        {
            _key = key;
        }

        public RegistryKeyWrapper()
        {
            
        }

        public IEnumerable<string> GetSubKeyNames()
        {
            return _key.GetSubKeyNames();
        }

        public IRegistryKeyWrapper OpenSubKey(string keyName)
        {
            return new RegistryKeyWrapper(_key.OpenSubKey(keyName));
        }

        public object GetValue(string name)
        {
            return _key.GetValue(name);
        }

        public void Close()
        {
            _key.Close();
        }

        public string Name
        {
            get { return _key.Name; }
        }
    }
}