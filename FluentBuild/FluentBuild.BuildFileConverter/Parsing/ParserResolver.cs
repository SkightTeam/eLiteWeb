namespace FluentBuild.BuildFileConverter.Parsing
{
    public interface IParserResolver
    {
        ITaskParser Resolve(string name);
    }

    public class ParserResolver : IParserResolver
    {
        public ITaskParser Resolve(string name)
        {
            switch (name)
            {
                case "csc":
                    return new CscParser();
                case "call":
                    return new CallParser();
                case "asminfo":
                    return new AsmInfoParser();
                default:
                    return new UnkownTypeParser();
            }
        }
    }
}