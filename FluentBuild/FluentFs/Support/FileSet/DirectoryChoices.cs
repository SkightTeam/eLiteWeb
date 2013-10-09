namespace FluentFs.Support.FileSet
{
    ///<summary>
    /// Allows the user to pick certain attributes to add to a folder
    ///</summary>
    public class DirectoryChoices : Core.FileSet
    {
        private readonly bool _isInclusion;
        private Core.FileSet _fileset;

        protected internal override string PendingInclude
        {
            get { return _fileset.PendingInclude; }
            set { _fileset.PendingInclude = value; }
        }

        protected internal override string PendingExclude
        {
            get { return _fileset.PendingExclude; }
            set { _fileset.PendingExclude = value; }
        }

        internal DirectoryChoices(Core.FileSet fileset, IFileSystemUtility utility, bool isInclusion) : base(utility)
        {
            _fileset = fileset;
            _isInclusion = isInclusion;
        }

        ///<summary>
        /// Modifies the current include to have a \\**\\ added to the end
        ///</summary>
        public DirectoryChoices RecurseAllSubDirectories
        {
            get
            {
                if (_isInclusion)
                    _fileset.PendingInclude = _fileset.PendingInclude + "\\**\\";
                else
                    _fileset.PendingExclude = _fileset.PendingExclude + "\\**\\";
                return this;
            }
        }

        ///<summary>
        /// Applies a filter to use when searching for files
        ///</summary>
        ///<param name="filter">A wildcard filter (e.g. *.cs)</param>
        public Core.FileSet Filter(string filter)
        {
            if (_isInclusion)
                _fileset.PendingInclude = _fileset.PendingInclude + "\\" + filter;
            else
                _fileset.PendingExclude = _fileset.PendingExclude + "\\" + filter;
            ProcessPendings();
            return this;
        }
    }
}