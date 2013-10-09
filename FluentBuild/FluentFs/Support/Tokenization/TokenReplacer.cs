using System.IO;


namespace FluentFs.Support.Tokenization
{
    ///<summary>
    /// Replaces tokens in a file
    ///</summary>
    public class TokenReplacer
    {
        private readonly IFileSystemWrapper _fileSystemWrapper;
        internal string Input;
        internal string Token;
        internal string Delimeter;

        internal TokenReplacer(string input) : this(new FileSystemWrapper(), input)
        {
        }

        internal TokenReplacer(IFileSystemWrapper fileSystemWrapper, string input)
        {
            _fileSystemWrapper = fileSystemWrapper;
            Input = input;
            Delimeter = "@";
        }


        ///<summary>
        /// Replaces a token in a string using a default delimiter of @
        ///</summary>
        ///<param name="token">the token to replace (without the delimiter)</param>
        ///<returns></returns>
        public TokenWith ReplaceToken(string token)
        {
            Token = token;
            return new TokenWith(this);
        }

        ///<summary>
        /// Replaces a token in a string
        ///</summary>
        ///<param name="token">the token to replace (without the delimiter)</param>
        ///<param name="delimiter">the delimiter that surrounds the token</param>
        ///<returns></returns>
        public TokenWith ReplaceToken(string token, string delimiter)
        {
            Token = token;
            return new TokenWith(this, delimiter);
        }


        ///<summary>
        /// Outputs the token replaced to a file
        ///</summary>
        ///<param name="destination">the destination path</param>
        ///<exception cref="IOException">Occurs if the file already exists</exception>
        public void To(string destination)
        {
            if (_fileSystemWrapper.FileExists(destination))
                throw new IOException("File already exists. Delete it first");
            _fileSystemWrapper.WriteAllText(destination, Input);
        }

        public override string ToString()
        {
            return Input;
        }
    }
}