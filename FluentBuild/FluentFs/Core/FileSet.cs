using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using FluentFs.Support.FileSet;

namespace FluentFs.Core
{
    ///<summary>
    /// Represents a series of files
    ///</summary>
    public interface IFileSet
    {
        ///<summary>
        /// Returns a list of files that is contained within the fileset. 
        ///</summary>
        ReadOnlyCollection<string> Files { get; }

        ///<summary>
        /// Copies the fileset
        ///</summary>
        CopyFileset Copy { get; }

        ///<summary>
        /// Includes a path in the fileset.
        ///</summary>
        ///<param name="path">path to files to include</param>
        FileSet Include(File path);
        
        ///<summary>
        /// Includes a path in the fileset.
        ///</summary>
        ///<param name="path">path to files to include</param>
        FileSet Include(string path);

        ///<summary>
        /// Excludes a path in the fileset.
        ///</summary>
        ///<param name="path">path to files to exclude</param>
        FileSet Exclude(string path);

        ///<summary>
        /// Sets the include to be a folder and opens additional options
        ///</summary>
        ///<param name="path">The buildfolder representing the path to be used</param>
        FileSet Include(Directory path);

        ///<summary>
        /// Sets the exclude to be a folder and opens additional options
        ///</summary>
        ///<param name="path">The buildfolder representing the path to be used</param>
        FileSet Exclude(Directory path);

        ///<summary>
        /// Sets the exclude to be a file and opens additional options
        ///</summary>
        ///<param name="path">The file representing the path to be used</param>
        FileSet Exclude(File path);
    }

    ///<summary>
    /// Represents a set of files with include and exclude filters
    ///</summary>
    public class FileSet : IFileSet
    {
        private readonly IFileSystemUtility _utility;
        internal List<string> Exclusions = new List<string>();
        internal List<string> Inclusions = new List<string>();

        private bool _isInclusion;

        ///<summary>
        /// Creates a new fileset
        ///</summary>
        public FileSet() : this(new FileSystemUtility())
        {
        }

        internal FileSet(IFileSystemUtility utility)
        {
            _utility = utility;
        }

        protected internal virtual string PendingInclude { get; set; }
        protected internal virtual string PendingExclude { get; set; }


        ///<summary>
        /// Modifies the current include to have a \\**\\ added to the end
        ///</summary>
        public FileSet RecurseAllSubDirectories
        {
            get
            {
                if (_isInclusion)
                    PendingInclude = Path.Combine(PendingInclude, "**");
                else
                    PendingExclude = Path.Combine(PendingExclude,"**");
                return this;
            }
        }

        #region IFileSet Members

        public ReadOnlyCollection<string> Files
        {
            get
            {
                ProcessPendings();
                var files = new List<string>();
                files.AddRange(DetermineActualFiles(Inclusions));
                foreach (string exclusion in DetermineActualFiles(Exclusions))
                {
                    files.Remove(exclusion);
                }
                return files.AsReadOnly();
            }
        }

        public FileSet Include(File path)
        {
            return ProcessInclude(path.ToString());
        }

        public FileSet Include(Directory path)
        {
            _isInclusion = true;
            ProcessPendings();
            PendingInclude = path.ToString();
            return this;
        }

        public FileSet Include(string path)
        {
            ProcessPendings();
            Inclusions.Add(path);
            return this;
        }

        private FileSet ProcessInclude(string path)
        {
            ProcessPendings();
            Inclusions.Add(path);
            return this;
        }

        public FileSet Exclude(File path)
        {
            _isInclusion = false;
            ProcessPendings();
            PendingExclude = path.ToString();
            return this;
        }

        public FileSet Exclude(Directory path)
        {
            _isInclusion = false;
            ProcessPendings();
            PendingExclude = path.ToString();
            return this;
        }

        public FileSet Exclude(string path)
        {
            _isInclusion = false;
            ProcessPendings();
            PendingExclude = path;
            return this;
        }

        private FileSet ProcessExclude(string path)
        {
            ProcessPendings();
            Exclusions.Add(path);
            return this;
        }

        public CopyFileset Copy
        {
            get
            {
                ProcessPendings();
                return new CopyFileset(this);
            }
        }

        #endregion

        internal IEnumerable<string> DetermineActualFiles(List<string> input)
        {
            foreach (string path in input)
            {
                if (path.IndexOf('*') == -1)
                {
                    yield return path;
                }
                else
                {
                    IList<string> allFilesMatching = _utility.GetAllFilesMatching(path);
                    if (allFilesMatching != null)
                    {
                        foreach (string match in allFilesMatching)
                        {
                            yield return match;
                        }
                    }
                }
            }
        }

        ///<summary>
        /// Applies a filter to use when searching for files
        ///</summary>
        ///<param name="filter">A wildcard filter (e.g. *.cs)</param>
        public FileSet Filter(string filter)
        {
            if (_isInclusion)
                PendingInclude = Path.Combine(PendingInclude, filter);
            else
                PendingExclude = Path.Combine(PendingExclude,filter);
            ProcessPendings();
            return this;
        }

    


        internal void ProcessPendings()
        {
            if (!string.IsNullOrEmpty(PendingExclude))
            {
                string tmp = PendingExclude;
                PendingExclude = string.Empty;
                ProcessExclude(tmp);
            }

            if (!string.IsNullOrEmpty(PendingInclude))
            {
                string tmp = PendingInclude;
                PendingInclude = string.Empty;
                ProcessInclude(tmp);
            }
        }

      
    }
}