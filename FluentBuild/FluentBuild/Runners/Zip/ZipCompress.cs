using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;

using FluentBuild.Utilities;
using ICSharpCode.SharpZipLib.Zip;

namespace FluentBuild.Runners.Zip
{
    public interface IZipCompress
    {
        ///<summary>
        /// Determine the compression level to use from 1(lowest) to 9(highest)
        ///</summary>
        OneThroughNine UsingCompressionLevel { get; }

        ///<summary>
        /// Sets the password to protect the zip file with
        ///</summary>
        ///<param name="password">The password to use</param>
        ZipCompress UsingPassword(string password);

        ///<summary>
        /// Sets ZipCompress to compress a single file
        ///</summary>
        ///<param name="file">The file to compress</param>
        ZipCompress SourceFile(FluentFs.Core.File file);

        ///<summary>
        /// Sets ZipCompress to compress a single file
        ///</summary>
        ///<param name="file">The file to compress</param>
        ZipCompress SourceFile(string file);

        ///<summary>
        /// Sets ZipCompress to compress an entire folder
        ///</summary>
        ///<param name="path">The folder to compress</param>
        ZipCompress SourceFolder(FluentFs.Core.Directory path);

        ///<summary>
        /// Sets ZipCompress to compress an entire folder
        ///</summary>
        ///<param name="path">The folder to compress</param>
        ZipCompress SourceFolder(string path);

        ///<summary>
        /// The location to place the output
        ///</summary>
        ///<param name="zipFilePath">path to the output file</param>
        void To(FluentFs.Core.File zipFilePath);

        ///<summary>
        /// The location to place the output
        ///</summary>
        ///<param name="zipFilePath">path to the output file</param>
        ZipCompress To(string zipFilePath);
    }

    ///<summary>
    /// Compresses a file or folder
    ///</summary>
    public class ZipCompress : InternalExecutable, IZipCompress
    {
        private readonly IFileSystemHelper _fileSystemHelper;
        internal int CompressionLevel;
        internal string _password;
        internal string _file;
        internal string _path;
        private string _outputPath;

        internal ZipCompress(IFileSystemHelper fileSystemHelper)
        {
            _fileSystemHelper = fileSystemHelper;
        }

        public ZipCompress() : this(new FileSystemHelper())
        {
        }

        ///<summary>
        /// Determine the compression level to use from 1(lowest) to 9(highest)
        ///</summary>
        public OneThroughNine UsingCompressionLevel { get { return new OneThroughNine(this);} }

        ///<summary>
        /// Sets the password to protect the zip file with
        ///</summary>
        ///<param name="password">The password to use</param>
        
        public ZipCompress UsingPassword(string password)
        {
            _password = password;
            return this;
        }

        ///<summary>
        /// Sets ZipCompress to compress a single file
        ///</summary>
        ///<param name="file">The file to compress</param>
        public ZipCompress SourceFile(FluentFs.Core.File file)
        {
            return SourceFile(file.ToString());
        }

        ///<summary>
        /// Sets ZipCompress to compress a single file
        ///</summary>
        ///<param name="file">The file to compress</param>
        public ZipCompress SourceFile(string file)
        {
            _file = file;
            return this;
        }

        ///<summary>
        /// Sets ZipCompress to compress an entire folder
        ///</summary>
        ///<param name="path">The folder to compress</param>
        public ZipCompress SourceFolder(FluentFs.Core.Directory path)
        {
            return SourceFolder(path.ToString());
        }

        ///<summary>
        /// Sets ZipCompress to compress an entire folder
        ///</summary>
        ///<param name="path">The folder to compress</param>
        public ZipCompress SourceFolder(string path)
        {
            _path = path;
            return this;
        }

        internal IList<String> GetFiles()
        {
            //only one should be set
            if (String.IsNullOrEmpty(_file) && (String.IsNullOrEmpty(_path)))
                throw new ArgumentException("sourceFile OR sourceFolder must be set");

            if (!String.IsNullOrEmpty(_file) && (!String.IsNullOrEmpty(_path)))
                throw new ArgumentException("sourceFile OR sourceFolder must be set");

            if (!string.IsNullOrEmpty(_file))
                return new List<String> {_file};

            return _fileSystemHelper.FindInFoldersRecursively(_path, "*.*");
        }


        ///<summary>
        /// The location to place the output
        ///</summary>
        ///<param name="zipFilePath">path to the output file</param>
        public void To(FluentFs.Core.File zipFilePath)
        {
            To(zipFilePath.ToString());
        }

        ///<summary>
        /// The location to place the output
        ///</summary>
        ///<param name="zipFilePath">path to the output file</param>
        public ZipCompress To(string zipFilePath)
        {
            _outputPath = zipFilePath;
            return this;
        }

        internal override void InternalExecute()
        {
            using (var zipOut = new ZipOutputStream(_fileSystemHelper.CreateFile(_outputPath)))
            {
                zipOut.SetLevel(CompressionLevel);
                zipOut.Password = _password;

                foreach (string fileName in GetFiles())
                {

                    //strip of the base folder 
                    //this will keep folders preserved
                    string path;
                    if (_path == null) //we are only compressing a single file
                        path = fileName;
                    else
                        path = fileName.Replace(_path, "");

                    if (path.StartsWith("\\"))
                        path = path.Substring(1); //removes the leading \

                    var entry = new ZipEntry(path);
                    Stream sReader = _fileSystemHelper.ReadFile(fileName);
                    var buff = new byte[Convert.ToInt32(sReader.Length)];
                    sReader.Read(buff, 0, (int)sReader.Length);

                    var fileInfo = new FileInfo(fileName);
                    entry.DateTime = fileInfo.LastWriteTime;
                    entry.Size = sReader.Length;
                    sReader.Close();
                    zipOut.PutNextEntry(entry);
                    zipOut.Write(buff, 0, buff.Length);
                }
                zipOut.Finish();
                zipOut.Close();
            }
        }
    }
}