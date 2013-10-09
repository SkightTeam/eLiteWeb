using System;
using FluentBuild.Utilities;

namespace FluentBuild.AssemblyInfoBuilding
{
    ///<summary>
    /// Determines which language to use to generate an assembly info file
    ///</summary>
    public class AssemblyInfoLanguage
    {
        private readonly IActionExcecutor _executor;

        internal AssemblyInfoLanguage(IActionExcecutor executor)
        {
            _executor = executor;
        }

        internal AssemblyInfoLanguage() : this(new ActionExcecutor())
        {
        }

        /// <summary>
        /// Generate using C#
        /// </summary>
        public void CSharp(Action<IAssemblyInfoDetails> args)
        {
            _executor.Execute<AssemblyInfoDetails, CSharpAssemblyInfoBuilder>(args, new CSharpAssemblyInfoBuilder());
        }

        /// <summary>
        /// Generate using Visual Basic
        /// </summary>
        public void VisualBasic(Action<IAssemblyInfoDetails> args)
        {
            _executor.Execute<AssemblyInfoDetails, VisualBasicAssemblyInfoBuilder>(args, new VisualBasicAssemblyInfoBuilder());
        }
    }
}