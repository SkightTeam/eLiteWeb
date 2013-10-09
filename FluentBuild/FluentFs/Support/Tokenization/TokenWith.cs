using System;

namespace FluentFs.Support.Tokenization
{
    ///<summary>
    /// Sets the replacement text
    ///</summary>
    public class TokenWith
    {
        private readonly TokenReplacer _replacer;
        private string _delimiter;

        internal TokenWith(TokenReplacer replacer) : this(replacer, "@")
        {
            
        }

        internal TokenWith(TokenReplacer replacer, string delimiter)
        {
            this._replacer = replacer;
            this._delimiter = delimiter;
        }

        ///<summary>
        /// Replaces the input with a value
        ///</summary>
        ///<param name="value">The value to use in the replacement</param>
        public TokenReplacer With(string value)
        {
            _replacer.Input = _replacer.Input.Replace(String.Format("{0}{1}{0}", _delimiter, _replacer.Token), value);
            return _replacer;
        }


    }
}