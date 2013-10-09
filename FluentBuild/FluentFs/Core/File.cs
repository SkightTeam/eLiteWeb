using FluentFs.Support;

namespace FluentFs.Core
{
    ///<summary>
    /// Represents a file used or created in a build
    ///</summary>
    public class File
    {
        private readonly IFileSystemWrapper _fileSystemWrapper;
        public string Path { get; set; }

        ///<summary>
        ///</summary>
        ///<param name="path">Path to the file</param>
        public File(string path) : this(new FileSystemWrapper(), path)
        {
        }

        internal File(IFileSystemWrapper fileSystemWrapper, string path)
        {
            _fileSystemWrapper = fileSystemWrapper;
            Path = path;
        }

        ///<summary>
        /// Deletes the file 
        ///<remarks>If the file does not exist no error will be thrown (even if OnError is set to fail)</remarks>
        ///</summary>
        public void Delete()
        {
            Delete(OnError.Fail);
        }
        
        ///<summary>
        /// Moves a build artifact to a new location
        ///</summary>
        public MoveFileArtifact Move
        {
            get { return new MoveFileArtifact(this);}
        }

        ///<summary>
        /// Returns only the file name of the build artifact
        ///</summary>
        ///<returns>The filename of the artifact</returns>
        public string FileName()
        {
            return System.IO.Path.GetFileName(Path);
        }

        ///<summary>
        /// Deletes the file 
        ///<remarks>If the file does not exist no error will be thrown (even if OnError is set to fail)</remarks>
        /// <param name="onError">Sets wether to fail or continue if an error occurs</param>
        ///</summary>
        public void Delete(OnError onError)
        {
            FailableActionExecutor.DoAction(onError, _fileSystemWrapper.DeleteFile, Path);
        }

        
        /// <summary>
        /// Renames the artifact
        /// </summary>
        public RenameFile Rename
        {
            get { return new RenameFile(this); }
        }

        
        /// <summary>
        /// Copies the artifact
        /// </summary>
        public CopyFile Copy
        {
            get {
                return new CopyFile(this);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns>The build artifact path</returns>
        public override string ToString()
        {
            return Path;
        }
    }
}