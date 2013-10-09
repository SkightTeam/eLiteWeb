namespace FluentBuild.Database
{
    public class MsSqlConnection
    {
        public MsSqlUtilities WithConnectionString(string connectionString)
        {
            var engine = new MsSqlEngine(connectionString);
            return new MsSqlUtilities(engine);
        }
    }
}