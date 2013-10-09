namespace FluentBuild
{
    ///<summary>
    /// The verbosity of messages
    ///</summary>
    public enum VerbosityLevel
    {
        ///<summary>
        /// Nothing should be output
        ///</summary>
        None=0,
        ///<summary>
        /// Only task names should be output
        ///</summary>
        TaskNamesOnly=1,

        ///<summary>
        /// Task names and their details should be output
        ///</summary>
        TaskDetails=2, 
        
        ///<summary>
        /// Output everything possible.
        ///</summary>
        Full=3
    }
}