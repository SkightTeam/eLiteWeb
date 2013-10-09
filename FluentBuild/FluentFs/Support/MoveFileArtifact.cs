using System.IO;
using Directory = FluentFs.Core.Directory;
using File = FluentFs.Core.File;

namespace FluentFs.Support
{
    ///<summary>
    /// Moves a file artifact to a destination
    ///</summary>
    public class MoveFileArtifact : Failable<MoveFileArtifact>
    {
        private readonly File _file;
        private readonly IFileSystemWrapper _fileSystemWrapper;

        internal MoveFileArtifact(IFileSystemWrapper fileSystemWrapper, File file)
        {
            _fileSystemWrapper = fileSystemWrapper;
            _file = file;
        }

        internal MoveFileArtifact(File file)
            : this(new FileSystemWrapper(), file)
        {
        }

        ///<summary>
        /// Moves a file to a destination
        ///</summary>
        ///<param name="destination">the new location of the file</param>
        public File To(string destination)
        {
            if (_fileSystemWrapper.DirectoryExists(destination))
            {
                destination = Path.Combine(destination, _file.FileName());
            }

            FailableActionExecutor.DoAction(OnError, _fileSystemWrapper.MoveFile, _file.ToString(), destination);
            _file.Path = destination;
            return _file;
        }

        ///<summary>
        /// Moves a file to a destination
        ///</summary>
        ///<param name="destination">the new location of the file</param>
        public File To(File destination)
        {
            return To(destination.ToString());
        }

        ///<summary>
        /// Moves a file to a destination
        ///</summary>
        ///<param name="destination">the new location of the file</param>
        public File To(Directory destination)
        {
            return To(destination.ToString());
        }
    }
}