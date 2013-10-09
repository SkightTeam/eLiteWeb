using System;


namespace FluentBuild.Runners.Zip
{
    ///<summary>
    /// Choice class that determines to compress or decompress
    ///</summary>
    [Obsolete("Replaced with Task.Run.Zip",true)]
    public class Zip
    {
        internal Zip()
        {
            
        }

        ///<summary>
        /// Creates a ZipCompress object that is used to compress files
        ///</summary>
        public ZipCompress Compress { get { return new ZipCompress(); }}

        ///<summary>
        /// Creates a ZipDecompress object that is used to decompress files
        ///</summary>
        ///<param name="pathToArchive">Path to the zip file to decompress</param>
        public ZipDecompress Decompress(string pathToArchive)
        {
            return new ZipDecompress().Path(pathToArchive);
        }
    }
}