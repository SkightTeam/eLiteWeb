using System;
using System.Collections.Specialized;
using System.Data;
using System.Data.SqlClient;
using System.IO;


namespace FluentBuild.Database
{
    public interface IMsSqlEngine
    {
        string PathToCreateScript { get; set; }
        string PathToUpdateScripts { get; set; }
        string VersionTable { get; set; }
        bool DoesDatabaseAlreadyExist();
        void Execute();
        SqlConnectionStringBuilder ConnectionString { get; }
    }

    public class MsSqlEngine : IMsSqlEngine
    {
        internal readonly SqlConnectionStringBuilder _connectionString;
        private SqlConnectionStringBuilder _masterDatabaseConnectionString;

        public MsSqlEngine(string connectionString)
        {
            _connectionString = new SqlConnectionStringBuilder();
                    _connectionString.ConnectionString = connectionString;
                    _masterDatabaseConnectionString = new SqlConnectionStringBuilder(_connectionString.ConnectionString);
                    _masterDatabaseConnectionString.InitialCatalog = "master";
        }

        public string PathToCreateScript { get; set; }

        public string PathToUpdateScripts { get; set; }

        public string VersionTable { get; set; }

        public bool DoesDatabaseAlreadyExist()
        {
            using (IDbConnection con = new SqlConnection(_masterDatabaseConnectionString.ConnectionString))
            {
                var parameters = new NameValueCollection();
                parameters.Add("databaseName", _connectionString.InitialCatalog);
                using (IDbCommand command = CreateTextCommand(con, "SELECT Count(*) FROM master.sys.databases WHERE [name]=@databaseName", parameters))
                {
                    command.Connection.Open();
                    if ((int)command.ExecuteScalar() == 0)
                        return false;
                    return true;
                }
            }

        }

        private void ExecuteNonQueryCommandAgainstDatabase(SqlConnectionStringBuilder connection, string commandText, NameValueCollection parameters, bool useTransactions)
        {
            IDbTransaction transaction = null;
            using (IDbConnection con = new SqlConnection(connection.ConnectionString))
            {
                con.Open();
                if (useTransactions)
                    transaction = con.BeginTransaction();

                string separator = "GO" + Environment.NewLine;
                string seperator2 = "Go" + Environment.NewLine;
                string seperator3 = "go" + Environment.NewLine;
                string seperator4 = "gO" + Environment.NewLine;

                try
                {
                    commandText = commandText + Environment.NewLine; //adds a newline so the last line will always be GO\r\n
                    string[] commands = commandText.Split(new[] { separator, seperator2, seperator3, seperator4 }, StringSplitOptions.RemoveEmptyEntries);

                    if (commands.Length == 0) //it is only one statement that does not have a "GO" at the end
                        commands = new[] { commandText };

                    //execute each command in the array
                    foreach (string individualCommand in commands)
                    {
                        using (IDbCommand command = CreateTextCommand(con, individualCommand, parameters))
                        {
                            if (useTransactions)
                                command.Transaction = transaction;
                            Defaults.Logger.WriteDebugMessage("Executing:" + command.CommandText);
                            command.ExecuteNonQuery();
                        }
                    }

                    if (useTransactions)
                        transaction.Commit();
                }
                catch (Exception)
                {
                    if (useTransactions)
                        transaction.Rollback();
                    throw;
                }
            }
        }

        private IDbCommand CreateTextCommand(IDbConnection connection, string commandText, NameValueCollection parameters)
        {
            IDbCommand command = connection.CreateCommand();
            command.CommandType = CommandType.Text;
            command.CommandText = commandText;

            if (parameters == null)
                return command;

            foreach (object key in parameters.Keys)
            {
                IDbDataParameter tableNameParameter = command.CreateParameter();
                tableNameParameter.ParameterName = key.ToString();
                tableNameParameter.Value = parameters[key.ToString()];
                command.Parameters.Add(tableNameParameter);
            }
            return command;
        }

        public void Execute()
        {
            try
            {
                if (!DoesDatabaseAlreadyExist())
                {
                    CreateDatabase();
                }

                UpdateDatabase();
            }
            catch (Exception ex)
            {
                BuildFile.SetErrorState();
                Defaults.Logger.Write("ERROR", ex.ToString());
            }
        }

        public SqlConnectionStringBuilder ConnectionString
        {
            get { return _connectionString; }
        }

        private void UpdateDatabase()
        {
            Defaults.Logger.WriteDebugMessage("Executing database updates");
            //determine current version
            int currentVersion = 0;
            using (IDbConnection con = new SqlConnection(_connectionString.ConnectionString))
            {
                con.Open();
                IDbCommand command = CreateTextCommand(con, "select version from " + VersionTable, null);
                currentVersion = (int)command.ExecuteScalar();
            }

            //execute updates higher than that version
            foreach (string upgradeFile in System.IO.Directory.GetFiles(PathToUpdateScripts))
            {
                string fileName = Path.GetFileName(upgradeFile);
                int fileVersion = int.Parse(fileName.Substring(0, fileName.IndexOf("_")));
                if (fileVersion > currentVersion)
                {
                    Defaults.Logger.Write("DATABASE", "Upgrading database to version " + fileVersion);
                    using (var x = new StreamReader(upgradeFile))
                    {
                        ExecuteNonQueryCommandAgainstDatabase(_connectionString, x.ReadToEnd(), null, true);
                    }

                    ExecuteNonQueryCommandAgainstDatabase(_connectionString, "update " + VersionTable + " set version=" + fileVersion, null, true);
                }
            }
        }

        private void CreateDatabase()
        {
            Defaults.Logger.WriteDebugMessage("Database does not exist, creating it");
            //create database
            Defaults.Logger.WriteDebugMessage("creating database " + _connectionString.InitialCatalog);
            ExecuteNonQueryCommandAgainstDatabase(_masterDatabaseConnectionString, "CREATE DATABASE " + _connectionString.InitialCatalog, null, false);

            //execute create script
            Defaults.Logger.WriteDebugMessage("Executing create script");
            using (var x = new StreamReader(PathToCreateScript))
            {
                ExecuteNonQueryCommandAgainstDatabase(_connectionString, x.ReadToEnd(), null, true);
            }

            //create version table
            Defaults.Logger.WriteDebugMessage("Creating version table");
            ExecuteNonQueryCommandAgainstDatabase(_connectionString, "create table " + VersionTable + " (version int)", null, true);

            //insert version 0
            ExecuteNonQueryCommandAgainstDatabase(_connectionString, "insert into " + VersionTable + " values (0)", null, true);
        }
    }

    //public class MsSqlDatabase
    //{

    //    private readonly SqlConnectionStringBuilder _connectionString = new SqlConnectionStringBuilder();

    //    private string _createScriptPath;
    //    private SqlConnectionStringBuilder _masterDatabaseConnectionString;
    //    private string _upgradScriptPath;
    //    private string _versionTableName = "Version";

    //    public static MsSqlDatabase CreateOrUpgradeDatabase()
    //    {
    //        return new MsSqlDatabase();
    //    }

    //    public MsSqlDatabase CreateScript(string path)
    //    {
    //        _createScriptPath = path;
    //        return this;
    //    }

    //    public MsSqlDatabase UpdateScripts(string path)
    //    {
    //        _upgradScriptPath = path;
    //        return this;
    //    }

    //    public MsSqlDatabase ConnectionString(string connectionString)
    //    {
    //        _connectionString.Clear();
    //        _connectionString.ConnectionString = connectionString;
    //        _masterDatabaseConnectionString = new SqlConnectionStringBuilder(_connectionString.ConnectionString);
    //        _masterDatabaseConnectionString.InitialCatalog = "master";
    //        return this;
    //    }

    //    public MsSqlDatabase VersionTable(string tableName)
    //    {
    //        _versionTableName = tableName;
    //        return this;
    //    }


    //    public bool DoesDatabaseAlreadyExist()
    //    {
    //        //create a copy and set the initial DB to be master so we can check if the DB exists
    //        using (IDbConnection con = new SqlConnection(_masterDatabaseConnectionString.ConnectionString))
    //        {
    //            var parameters = new NameValueCollection();
    //            parameters.Add("databaseName", _connectionString.InitialCatalog);
    //            using (IDbCommand command = CreateTextCommand(con, "SELECT Count(*) FROM master.sys.databases WHERE [name]=@databaseName", parameters))
    //            {
    //                command.Connection.Open();
    //                if ((int) command.ExecuteScalar() == 0)
    //                    return false;
    //                return true;
    //            }
    //        }
    //    }

    //    private void ExecuteNonQueryCommandAgainstDatabase(SqlConnectionStringBuilder connection, string commandText, NameValueCollection parameters, bool useTransactions)
    //    {
    //        IDbTransaction transaction = null;
    //        using (IDbConnection con = new SqlConnection(connection.ConnectionString))
    //        {
    //            con.Open();
    //            if (useTransactions)
    //                transaction = con.BeginTransaction();

    //            string separator = "GO" + Environment.NewLine;
    //            string seperator2 = "Go" + Environment.NewLine;
    //            string seperator3 = "go" + Environment.NewLine;
    //            string seperator4 = "gO" + Environment.NewLine;

    //            try
    //            {
    //                commandText = commandText + Environment.NewLine; //adds a newline so the last line will always be GO\r\n
    //                string[] commands = commandText.Split(new[] {separator, seperator2, seperator3, seperator4}, StringSplitOptions.RemoveEmptyEntries);

    //                if (commands.Length == 0) //it is only one statement that does not have a "GO" at the end
    //                    commands = new[] {commandText};

    //                //execute each command in the array
    //                foreach (string individualCommand in commands)
    //                {
    //                    using (IDbCommand command = CreateTextCommand(con, individualCommand, parameters))
    //                    {
    //                        if (useTransactions)
    //                            command.Transaction = transaction;
    //                        MessageLogger.WriteDebugMessage("Executing:" + command.CommandText);
    //                        command.ExecuteNonQuery();
    //                    }
    //                }

    //                if (useTransactions)
    //                    transaction.Commit();
    //            }
    //            catch (Exception)
    //            {
    //                if (useTransactions)
    //                    transaction.Rollback();
    //                throw;
    //            }
    //        }
    //    }

    //    private IDbCommand CreateTextCommand(IDbConnection connection, string commandText, NameValueCollection parameters)
    //    {
    //        IDbCommand command = connection.CreateCommand();
    //        command.CommandType = CommandType.Text;
    //        command.CommandText = commandText;

    //        if (parameters == null)
    //            return command;

    //        foreach (object key in parameters.Keys)
    //        {
    //            IDbDataParameter tableNameParameter = command.CreateParameter();
    //            tableNameParameter.ParameterName = key.ToString();
    //            tableNameParameter.Value = parameters[key.ToString()];
    //            command.Parameters.Add(tableNameParameter);
    //        }
    //        return command;
    //    }

    //    public void Execute()
    //    {
    //        if (Environment.ExitCode != 0)
    //            return;

    //        try
    //        {
    //            if (!DoesDatabaseAlreadyExist())
    //            {
    //                CreateDatabase();
    //            }

    //            UpdateDatabase();
    //        } 
    //        catch (Exception ex)
    //        {
    //            Environment.ExitCode = 1;
    //            MessageLogger.Write("ERROR", ex.ToString());
    //        }
    //    }

    //    private void UpdateDatabase()
    //    {
    //        MessageLogger.WriteDebugMessage("Executing database updates");
    //        //determine current version
    //        int currentVersion = 0;
    //        using (IDbConnection con = new SqlConnection(_connectionString.ConnectionString))
    //        {
    //            con.Open();
    //            IDbCommand command = CreateTextCommand(con, "select version from " + _versionTableName, null);
    //            currentVersion = (int) command.ExecuteScalar();
    //        }

    //        //execute updates higher than that version
    //        foreach (string upgradeFile in Directory.GetFiles(_upgradScriptPath))
    //        {
    //            string fileName = Path.GetFileName(upgradeFile);
    //            int fileVersion = int.Parse(fileName.Substring(0, fileName.IndexOf("_")));
    //            if (fileVersion > currentVersion)
    //            {
    //                MessageLogger.Write("DATABASE", "Upgrading database to version " + fileVersion);
    //                using (var x = new StreamReader(upgradeFile))
    //                {
    //                    ExecuteNonQueryCommandAgainstDatabase(_connectionString, x.ReadToEnd(), null, true);
    //                }

    //                ExecuteNonQueryCommandAgainstDatabase(_connectionString, "update " + _versionTableName + " set version=" + fileVersion, null, true);
    //            }
    //        }
    //    }

    //    private void CreateDatabase()
    //    {
    //        MessageLogger.WriteDebugMessage("Database does not exist, creating it");
    //        //create database
    //        MessageLogger.WriteDebugMessage("creating database " + _connectionString.InitialCatalog);
    //        ExecuteNonQueryCommandAgainstDatabase(_masterDatabaseConnectionString, "CREATE DATABASE " + _connectionString.InitialCatalog, null, false);

    //        //execute create script
    //        MessageLogger.WriteDebugMessage("Executing create script");
    //        using (var x = new StreamReader(_createScriptPath))
    //        {
    //            ExecuteNonQueryCommandAgainstDatabase(_connectionString, x.ReadToEnd(), null, true);
    //        }

    //        //create version table
    //        MessageLogger.WriteDebugMessage("Creating version table");
    //        ExecuteNonQueryCommandAgainstDatabase(_connectionString, "create table " + _versionTableName + " (version int)", null, true);

    //        //insert version 0
    //        ExecuteNonQueryCommandAgainstDatabase(_connectionString, "insert into " + _versionTableName + " values (0)", null, true);
    //    }
    //}
}