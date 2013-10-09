namespace FluentBuild.Utilities
{
    ///<summary>
    /// Used to set the behavior of what to do on errors
    ///</summary>
    public enum OnError
    {
        ///<summary>
        /// Pass along the exception that was thrown
        ///</summary>
        Fail,
        ///<summary>
        /// Swallow errors
        ///</summary>
        Continue
    }
}