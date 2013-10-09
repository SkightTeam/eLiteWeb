using System;
using System.IO;
using FluentFs.Support.Tokenization;
using Directory = FluentFs.Core.Directory;
using File = FluentFs.Core.File;

namespace FluentFs.Support
{
    ///<summary>
    /// Copies a file
    ///</summary>
    public class CopyFile : Failable<CopyFile>
    {
        private readonly IFileSystemWrapper _fileSystemWrapper;
        private readonly File _source;

        internal CopyFile(IFileSystemWrapper fileSystemWrapper, File artifact)
        {
            _fileSystemWrapper = fileSystemWrapper;
            _source = artifact;
        }

        internal CopyFile(File artifact) : this(new FileSystemWrapper(), artifact)
        {
        }

        ///<summary>
        /// Destination
        ///</summary>
        ///<param name="destination">The destination</param>
        public void To(File destination)
        {
            To(destination.ToString());
        }

        ///<summary>
        /// Destination
        ///</summary>
        ///<param name="destination">The destination</param>
        public void To(Directory destination)
        {
            To(destination.ToString());
        }


        ///<summary>
        /// Destination
        ///</summary>
        ///<param name="destination">The destination path</param>
        public void To(String destination)
        {
            string destinationFileName;
            string destinationDirectory;
            //if no filename in destination then get it from the source
            
            if (!Path.HasExtension(destination))
            {
                destinationFileName = Path.GetFileName(_source.ToString());
                destinationDirectory = destination;
            }
            else
            {
                destinationFileName = Path.GetFileName(destination);
                destinationDirectory = Path.GetDirectoryName(destination);
            }

// ReSharper disable AssignNullToNotNullAttribute
            var dest = Path.Combine(destinationDirectory, destinationFileName);
// ReSharper restore AssignNullToNotNullAttribute
            FailableActionExecutor.DoAction(base.OnError, _fileSystemWrapper.Copy, _source.ToString(), dest);
        }


        /// <summary>
        /// Replaces a token in a file 
        /// </summary>
        /// <param name="token">the token to be replaced</param>
        /// <returns></returns>
        /// <example>tokens in the file are surrounded by @ signs. So ReplaceToken("name") would replace everything in a file with @name@</example>
        public TokenWith ReplaceToken(string token)
        {
            return new TokenReplacer(_fileSystemWrapper.ReadAllText(_source.ToString())).ReplaceToken(token);
        }
    }
}