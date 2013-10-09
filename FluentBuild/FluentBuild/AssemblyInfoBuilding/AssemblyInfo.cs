namespace FluentBuild.AssemblyInfoBuilding
{
    public interface IAssemblyInfo
    {
        /// <summary>
        /// select the language used to generate the assembly info file
        /// </summary>
        AssemblyInfoLanguage Language { get; }
    }

    /// <summary>
    /// Allows the creation of assembly info files
    /// </summary>
    internal class AssemblyInfo : IAssemblyInfo
    {
        /// <summary>
        /// select the language used to generate the assembly info file
        /// </summary>
        public AssemblyInfoLanguage Language
        {
            get { return new AssemblyInfoLanguage(); }
        }

        internal AssemblyInfo()
        {
        }
    }
}