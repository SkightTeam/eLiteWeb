using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using FluentFs.Support;


namespace FluentBuild.Utilities
{
    internal interface IFileSystemHelper
    {
        string Find(string fileName, string directory);
        string Find(string fileName);
        List<String> FindInFoldersRecursively(string directory, string filter);
        Stream CreateFile(string path);
        Stream ReadFile(string path);
    }

    ///<summary>
    /// Finds files recursevely in a directory
    ///</summary>
    public class FileSystemHelper : IFileSystemHelper
    {
        private readonly IFileSystemWrapper _fileSystem;

        internal FileSystemHelper(IFileSystemWrapper fileSystem)
        {
            _fileSystem = fileSystem;
        }

        ///<summary>
        /// Instantiates a new FileSystemHelper
        ///</summary>
        public FileSystemHelper() : this(new FileSystemWrapper())
        {
        }

        ///<summary>
        /// Finds a file in a directory and searches subdirectories
        ///</summary>
        ///<param name="fileName">The filename to find</param>
        ///<param name="directory">The starting directory to search</param>
        ///<returns></returns>
        public string Find(string fileName, string directory)
        {
            IEnumerable<string> filesInDirectory = _fileSystem.GetFilesIn(directory);
            if (filesInDirectory != null)
            {
                foreach (string file in filesInDirectory)
                {
                    if (Path.GetFileName(file) == fileName)
                    {
                        return file;
                    }
                }
            }


            IEnumerable<string> subDirectories = _fileSystem.GetDirectories(directory);
            if (subDirectories != null)
            {
                foreach (string subDirectory in subDirectories)
                {
                    var find = Find(fileName, subDirectory);
                    if (find!= null)
                        return find;
                }
            }
            return null;
        }

        ///<summary>
        /// Finds a file in the CURRENT directory and searches subdirectories
        ///</summary>
        ///<param name="fileName">The filename to search for</param>
        ///<returns></returns>
        public string Find(string fileName)
        {
            return Find(fileName, Properties.CurrentDirectory);
        }

        public List<string> FindInFoldersRecursively(string directory, string filter)
        {
            return System.IO.Directory.GetFiles(directory, "*.*", SearchOption.AllDirectories).ToList();
        }

        public Stream CreateFile(string path)
        {
            return System.IO.File.Create(path);
        }

        public Stream ReadFile(string path)
        {
            return System.IO.File.OpenRead(path);
        }
    }
}