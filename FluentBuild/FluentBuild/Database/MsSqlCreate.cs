namespace FluentBuild.Database
{
    public class MsSqlCreate
    {
        private readonly IMsSqlEngine _engine;

        public MsSqlCreate(IMsSqlEngine engine)
        {
            _engine = engine;
        }

        public MsSqlUpdate PathToUpdateScripts(string path)
        {
            _engine.PathToUpdateScripts = path;
            return new MsSqlUpdate(_engine);
        }
    }
}