using Microsoft.Win32;

namespace FluentBuild.Utilities
{
    internal interface IRegistryWrapper
    {
        IRegistryKeyWrapper OpenLocalMachineKey(string key);
    }


    /// <summary>
    /// Wrapper around registry access to provide testability
    /// </summary>
    internal class RegistryWrapper : IRegistryWrapper
    {
        public IRegistryKeyWrapper OpenLocalMachineKey(string key)
        {
            RegistryKey subKey = Registry.LocalMachine.OpenSubKey(key);
            if (subKey==null)
                return null;
            return new RegistryKeyWrapper(subKey);
        }
    }
}