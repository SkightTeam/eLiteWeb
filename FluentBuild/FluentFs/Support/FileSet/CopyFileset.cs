using System.IO;
using Directory = FluentFs.Core.Directory;

namespace FluentFs.Support.FileSet
{
    ///<summary>
    /// Copies the fileset
    ///</summary>
    public class CopyFileset : Failable<CopyFileset>
    {
        private readonly Core.FileSet _fileSet;
        private readonly IFileSystemWrapper _fileSystemWrapper;

        internal CopyFileset(Core.FileSet fileSet, IFileSystemWrapper fileSystemWrapper)
        {
            _fileSet = fileSet;
            _fileSystemWrapper = fileSystemWrapper;
        }

        internal CopyFileset(Core.FileSet fileSet): this(fileSet, new FileSystemWrapper())
        {
        }

        ///<summary>
        /// Copies the fileset to the destination
        ///</summary>
        ///<param name="destination">A Directory that will recieve the files</param>
        ///<returns></returns>
        public Core.FileSet To(Core.Directory destination)
        {
            return To(destination.ToString());
        }

        ///<summary>
        /// Copies the fileset to the destination
        ///</summary>
        ///<param name="destination">Path to a folder that will recieve the files</param>
        ///<returns></returns>
        public Core.FileSet To(string destination)
        {
//            Logger.Write("copy", String.Format("Copying {0} files to '{1}'", _fileSet.Files.Count, destination));
            foreach (string file in _fileSet.Files)
            {
  //              Logger.WriteDebugMessage(String.Format("Path: {0}\\{1}", destination, Path.GetFileName(file)));
                string destinationPath = Path.Combine(destination.ToString(), Path.GetFileName(file));
                FailableActionExecutor.DoAction<string, string>(this.OnError, _fileSystemWrapper.Copy, file, destinationPath);
            }
            return _fileSet;
        }

    }
}
