namespace FluentBuild.Database
{
    public class MsSqlUpdate
    {
        private readonly IMsSqlEngine _engine;

        public MsSqlUpdate(IMsSqlEngine engine)
        {
            _engine = engine;
        }

        public MsSqlVersionTable VersionTable(string tableName)
        {
            _engine.VersionTable = tableName;
            return new MsSqlVersionTable(_engine);
        }
    }
}