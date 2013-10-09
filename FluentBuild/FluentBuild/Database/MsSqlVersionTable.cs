namespace FluentBuild.Database
{
    public class MsSqlVersionTable
    {
        private readonly IMsSqlEngine _engine;

        public MsSqlVersionTable(IMsSqlEngine engine)
        {
            _engine = engine;
        }

        public void Execute()
        {
            _engine.Execute();
        }
    }
}