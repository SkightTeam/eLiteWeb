using System.IO;
using FluentFs.Support;
using FluentFs.Support.FileSet;

namespace FluentFs.Core
{

    /// <summary>
    /// Represents a folder
    /// </summary>
    public class Directory
    {
        private readonly IFileSystemWrapper _fileSystemWrapper;
        private readonly IFileSetFactory _fileSetFactory;
        private readonly string _path;

        internal Directory(IFileSystemWrapper fileSystemWrapper, IFileSetFactory fileSetFactory, string path)
        {
            _fileSystemWrapper = fileSystemWrapper;
            _fileSetFactory = fileSetFactory;
            _path = path;
        }

        ///<summary>
        /// Creates a new Directory object
        ///</summary>
        ///<param name="path">Path to the folder</param>
        public Directory(string path) : this(new FileSystemWrapper(), new FileSetFactory(),  path)
        {            
        }

        /// <summary>
        /// Creates a new Directory object for a subdirectory
        /// </summary>
        /// <param name="path">The subfolder below the current Directory</param>
        /// <returns>a new Directory object</returns>
        /// <remarks>The folder does not need to exist to use this method</remarks>
        public Directory SubFolder(string path)
        {
            return new Directory(Path.Combine(_path, path));
        }



        /// <summary>
        /// Deletes the folder. If the the folder can not be deleted, or does not exist then an exception is thrown.
        /// </summary>
        /// <returns></returns>
        public Directory Delete()
        {
            return Delete(OnError.Fail);
        }

        /// <summary>
        /// Deletes the folder.
        /// </summary>
        ///<param name="onError">Sets the behavior of how to handle an error</param>
        ///<returns></returns>
        public Directory Delete(OnError onError)
        {
            FailableActionExecutor.DoAction(onError, _fileSystemWrapper.DeleteDirectory, _path, true);
            return this;
        }

        ///<summary>
        /// Creates the folder
        ///</summary>
        ///<returns></returns>
        public Directory Create()
        {
            return Create(OnError.Fail);
        }
       
        ///<summary>
        /// Creates the folder
        ///</summary>
        ///<param name="onError">Allows you to set the error behavior</param>
        ///<returns></returns>
        public Directory Create(OnError onError)
        {
            FailableActionExecutor.DoAction(onError, _fileSystemWrapper.CreateDirectory, _path);
            return this;
        }

        /// <summary>
        /// Provides the current path internal to the Directory object
        /// </summary>
        /// <returns>The path of the Directory</returns>
        public override string ToString()
        {
            return _path;
        }

        /// <summary>
        /// Appends a filename onto the Directory
        /// </summary>
        /// <param name="name">The name (or filter) of the file</param>
        /// <returns>A File that represents the full path to the file</returns>
        public File File(string name)
        {
            return new File(Path.Combine(_path, name));
        }

        ///<summary>
        /// Creates a fileset based on a filter
        ///</summary>
        ///<param name="filter">A filter that can contain wildcards</param>
        ///<returns></returns>
        public Core.FileSet Files(string filter)
        {
            var fileSet = _fileSetFactory.Create();
            return fileSet.Include(new Directory(_path)).Filter(filter);
        }

        /// <summary>
        /// Gets all files in a directory
        /// </summary>
        /// <returns>A fileset containing all files in a directory</returns>
        public Core.FileSet Files()
        {
            var fileSet = _fileSetFactory.Create();
            return fileSet.Include(new Directory(_path));
        }
    }
}