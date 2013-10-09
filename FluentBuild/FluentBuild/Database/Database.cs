namespace FluentBuild.Database
{
    public class Database
    {
        public static MsSqlConnection MsSqlDatabase
        {
            get { return new MsSqlConnection(); }
        }
    }
}