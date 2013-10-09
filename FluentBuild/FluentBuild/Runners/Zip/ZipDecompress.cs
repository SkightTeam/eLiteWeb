using System.IO;
using FluentBuild.Utilities;
using ICSharpCode.SharpZipLib.Zip;

namespace FluentBuild.Runners.Zip
{
    ///<summary>
    /// Zip decompresses an archive
    ///</summary>
    public class ZipDecompress : InternalExecutable
    {
        internal readonly IFileSystemHelper _fileSystemHelper;
        internal string _pathToArchive;
        internal string _password;
        private string _outputPath;


        internal ZipDecompress(IFileSystemHelper fileSystemHelper)
        {
            _fileSystemHelper = fileSystemHelper;
        }

        public ZipDecompress() : this(new FileSystemHelper())
        {
        }

        ///<summary>
        /// Sets the password to decompress a file
        ///</summary>
        ///<param name="password">the password to use</param>
        public ZipDecompress UsingPassword(string password)
        {
            _password = password;
            return this;
        }

        ///<summary>
        /// Sets the output path
        ///</summary>
        ///<param name="outputPath">The path you would like the file(s) outputed to</param>
        public ZipDecompress To(string outputPath)
        {
            _outputPath = outputPath;
            return this;
        }

        public ZipDecompress Path(string zipFilePath)
        {
            _pathToArchive = zipFilePath;
            return this;
        }

        internal override void InternalExecute()
        {
            using (var zipInputStream = new ZipInputStream(_fileSystemHelper.ReadFile(_pathToArchive)))
            {
                zipInputStream.Password = _password;

                ZipEntry entry;
                while ((entry = zipInputStream.GetNextEntry()) != null)
                {
                    Stream streamWriter = _fileSystemHelper.CreateFile(System.IO.Path.Combine(_outputPath + "\\", entry.Name));
                    long size = entry.Size;
                    var data = new byte[size];
                    while (true)
                    {
                        size = zipInputStream.Read(data, 0, data.Length);
                        if (size > 0) streamWriter.Write(data, 0, (int)size);
                        else break;
                    }
                    streamWriter.Close();
                }
            }
        }
    }
}