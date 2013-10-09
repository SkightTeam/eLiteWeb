using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FluentBuild.Utilities
{
    internal interface IEnvironmentVariableWrapper
    {
        string Get(string name);
    }

    internal class EnvironmentVariableWrapper : IEnvironmentVariableWrapper
    {
        public string Get(string name)
        {
            return Environment.GetEnvironmentVariable(name);
        }
    }
}
