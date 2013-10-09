namespace FluentBuild.Database
{
    public class MsSqlUtilities
    {
        internal readonly IMsSqlEngine _engine;
        public MsSqlUtilities(IMsSqlEngine engine)
        {
            _engine = engine;
        }

        public bool DoesDatabaseAlreadyExist()
        {
            return _engine.DoesDatabaseAlreadyExist();
        }

        public MsSqlCreateOrUpdate CreateOrUpdate
        {
            get { return new MsSqlCreateOrUpdate(_engine); }
        }
    }
}