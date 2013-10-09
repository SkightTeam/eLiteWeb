using FluentFs.Core;

namespace FluentFs.Support
{
    ///<summary>
    /// Represents a class that has continue/fail on error behavior
    ///</summary>
    ///<typeparam name="T"></typeparam>
    public interface IFailable<T>
    {
        ///<summary>
        /// On error throw an exception
        ///</summary>
        T FailOnError { get; }

        ///<summary>
        /// Swallow exceptions and continue
        ///</summary>
        T ContinueOnError { get; }
    }

    ///<summary>
    /// Represents a class that has continue/fail on error behavior
    ///</summary>
    ///<typeparam name="T"></typeparam>
    public abstract class Failable<T> : IFailable<T>
    {
        protected internal OnError OnError;

        protected Failable()
        {
            OnError = OnError.Fail;
        }

        public T FailOnError
        {
            get
            {
                OnError = OnError.Fail;
                return (T)(object)this;
            }
        }

        public T ContinueOnError
        {
            get
            {
                OnError = OnError.Continue;
                return (T)(object)this;
            }
        }

    }
}