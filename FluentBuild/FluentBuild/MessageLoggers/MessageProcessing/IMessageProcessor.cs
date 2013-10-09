namespace FluentBuild.MessageLoggers.MessageProcessing
{
    public interface IMessageProcessor
    {
        void Display(string prefix, string output, string error, int exitCode);
    }
}