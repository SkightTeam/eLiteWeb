using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace FluentFs.Support.FileSet
{
    internal interface IFileSystemUtility
    {
        IList<string> GetAllFilesMatching(string filter);
    }

    internal class FileSystemUtility : IFileSystemUtility
    {
        private readonly ISearchPatternParser _parser;
        private readonly IFileSystemWrapper _fileSystemWrapper;

        public FileSystemUtility(ISearchPatternParser parser, IFileSystemWrapper fileSystemWrapper)
        {
            _parser = parser;
            _fileSystemWrapper = fileSystemWrapper;
        }

        public FileSystemUtility() : this(new SearchPatternParser(), new FileSystemWrapper())
        {
        }

        #region IFileSystemUtility Members

        public IList<string> GetAllFilesMatching(string filter)
        {
            Console.WriteLine(filter);
            Debug.WriteLine(filter);
            //a full file i.e. c:\temp\file1.txt
            if ((filter.LastIndexOf("*") == -1) && (Path.HasExtension(filter)))
            {
                var list = new List<String>();
                if (_fileSystemWrapper.FileExists(filter))
                    list.Add(filter);
                return list;
            }
            _parser.Parse(filter);
            return GetAllFilesMatching(_parser.Folder, _parser.SearchPattern, _parser.Recursive);
        }

        #endregion

        private IList<String> GetAllFilesMatching(string directory, string filter, bool recursive)
        {
            string[] files = System.IO.Directory.GetFiles(directory, filter);
            List<string> matching = files.ToList();
            if (recursive)
            {
                foreach (string subDirectory in _fileSystemWrapper.GetDirectories(directory))
                {
                    matching.AddRange(GetAllFilesMatching(subDirectory, filter, true));
                }
            }
            return matching;
        }
    }
}