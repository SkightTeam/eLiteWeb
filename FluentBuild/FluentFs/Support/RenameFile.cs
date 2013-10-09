using System.IO;
using File = FluentFs.Core.File;

namespace FluentFs.Support
{
    ///<summary>
    /// Renames a build artifact
    ///</summary>
    public class RenameFile : Failable<RenameFile>
    {
        private readonly File _file;
        private readonly IFileSystemWrapper _fileSystemWrapper;


        internal RenameFile(IFileSystemWrapper fileSystemWrapper, File file)
        {
            _fileSystemWrapper = fileSystemWrapper;
            _file = file;
        }

        internal RenameFile(File file) : this(new FileSystemWrapper(), file)
        {
        }

        ///<summary>
        /// Renames a file to a destination
        ///</summary>
        ///<param name="newName">the new name of the file</param>
        public File To(string newName)
        {
            var newPath = Path.GetDirectoryName(_file.ToString()) + "\\" + newName;
            FailableActionExecutor.DoAction(OnError, _fileSystemWrapper.MoveFile, _file.ToString(), newPath);
            _file.Path = newPath;
            return _file;
        }
    }
}