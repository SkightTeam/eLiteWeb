namespace FluentBuild.MessageLoggers.MessageProcessing
{
    internal class Message
    {
        public Message(string prefix, MessageType messageType)
        {
            Prefix = prefix;
            MessageType = messageType;
        }

        public MessageType MessageType { get; set; }

        public string Contents { get; set; }

        public string Prefix { get; set; }
    }
}