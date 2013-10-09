namespace FluentBuild.Database
{
    public class MsSqlCreateOrUpdate
    {
        private readonly IMsSqlEngine _engine;

        public MsSqlCreateOrUpdate(IMsSqlEngine engine)
        {
            _engine = engine;
        }

        public MsSqlCreate PathToCreateScript(string path)
        {
            _engine.PathToCreateScript = path;
            return new MsSqlCreate(_engine);
        }
    }
}