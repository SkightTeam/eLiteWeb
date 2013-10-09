namespace FluentBuild.Utilities
{
    public abstract class FailableInternalExecutable<T> : FluentBuild.Utilities.Failable<T>
    {
        internal abstract void InternalExecute();
    }
}