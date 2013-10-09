using System.Collections.Generic;

namespace FluentFs.Support
{
    public interface IFileSystemWrapper
    {
        void Copy(string source, string destination);
        bool FileExists(string path);
        void WriteAllText(string destination, string input);
        string ReadAllText(string path);
        IEnumerable<string> GetDirectories(string directory);
        void CreateDirectory(string path);
        bool DirectoryExists(string path);
        void DeleteDirectory(string path, bool recursive);
        void DeleteFile(string path);
        void MoveFile(string origin, string destination);
        IEnumerable<string> GetFilesIn(string directory);
    }

    public class FileSystemWrapper : IFileSystemWrapper
    {
        public void Copy(string source, string destination)
        {
            System.IO.File.Copy(source,destination);
        }

        public bool FileExists(string path)
        {
            return System.IO.File.Exists(path);
        }

        public void WriteAllText(string destination, string input)
        {
            System.IO.File.WriteAllText(destination, input);
        }

        public string ReadAllText(string path)
        {
            return System.IO.File.ReadAllText(path);
        }

        public IEnumerable<string> GetDirectories(string directory)
        {
            return System.IO.Directory.GetDirectories(directory);
        }

        public void CreateDirectory(string path)
        {
            System.IO.Directory.CreateDirectory(path);
        }

        public bool DirectoryExists(string path)
        {
            return System.IO.Directory.Exists(path);
        }

        public void DeleteDirectory(string path, bool recursive)
        {
            System.IO.Directory.Delete(path, recursive);
        }

        public void DeleteFile(string path)
        {
            System.IO.File.Delete(path);
        }

        public void MoveFile(string origin, string destination)
        {
            System.IO.File.Move(origin, destination);
        }

        public IEnumerable<string> GetFilesIn(string directory)
        {
            return System.IO.Directory.GetFiles(directory);
        }
    }
}