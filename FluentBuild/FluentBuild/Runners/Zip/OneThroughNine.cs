namespace FluentBuild.Runners.Zip
{
    ///<summary>
    /// Used to set the compression level for compressors
    ///</summary>
    public class OneThroughNine
    {
   
        private readonly ZipCompress _zipCompress;

        internal OneThroughNine(ZipCompress zipCompress)
        {
            _zipCompress = zipCompress;
        }

        ///<summary>
        ///</summary>
        public ZipCompress One
        {
            get
            {
                _zipCompress.CompressionLevel = 1;
                return _zipCompress;
            }
        }

        ///<summary>
        ///</summary>
        public ZipCompress Two
        {
            get
            {
                _zipCompress.CompressionLevel = 2;
                return _zipCompress;
            }
        }

        ///<summary>
        ///</summary>
        public ZipCompress Three
        {
            get
            {
                _zipCompress.CompressionLevel = 3;
                return _zipCompress;
            }
        }

        ///<summary>
        ///</summary>
        public ZipCompress Four
        {
            get
            {
                _zipCompress.CompressionLevel = 4;
                return _zipCompress;
            }
        }

        ///<summary>
        ///</summary>
        public ZipCompress Five
        {
            get
            {
                _zipCompress.CompressionLevel = 5;
                return _zipCompress;
            }
        }

        ///<summary>
        ///</summary>
        public ZipCompress Six
        {
            get
            {
                _zipCompress.CompressionLevel = 6;
                return _zipCompress;
            }
        }
        
        ///<summary>
        ///</summary>
        public ZipCompress Seven
        {
            get
            {
                _zipCompress.CompressionLevel = 7;
                return _zipCompress;
            }
        }

        ///<summary>
        ///</summary>
        public ZipCompress Eight
        {
            get
            {
                _zipCompress.CompressionLevel = 8;
                return _zipCompress;
            }
        }

        ///<summary>
        ///</summary>
        public ZipCompress Nine
        {
            get
            {
                _zipCompress.CompressionLevel = 9;
                return _zipCompress;
            }
        }


    }
}