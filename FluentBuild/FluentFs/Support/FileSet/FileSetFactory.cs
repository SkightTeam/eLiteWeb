using FluentFs.Core;

namespace FluentFs.Support.FileSet
{
    //Filesets often become created in code so this allows the mocking of filesets
    ///<summary>
    /// Creates a Fileset
    ///</summary>
    internal interface IFileSetFactory
    {
        ///<summary>
        /// Builds a fileset
        ///</summary>
        ///<returns>a fileset</returns>
        IFileSet Create();
    }

    ///<summary>
    /// Generates a new fileset
    ///</summary>
    internal class FileSetFactory : IFileSetFactory
    {
        public IFileSet Create()
        {
            return new Core.FileSet();
        }
    }
}
