using System.Configuration;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate;
using NHibernate.Caches.SysCache2;
using NHibernate.Tool.hbm2ddl;
using Skight.eLiteWeb.Domain.BasicExtensions;
using Skight.eLiteWeb.Domain.Containers;
using Configuration = NHibernate.Cfg.Configuration;

namespace Skight.eLiteWeb.Infrastructure.Persistent
{
    public class SessionProvider {
        private static SessionProvider _instance;

      // private readonly Assemblies assemblies;

        private readonly object lock_flag = new object();
        private ISessionFactory session_factory;
        private Configuration configuration;

        //public SessionProvider(Assemblies assemblies) {
        //    this.assemblies = assemblies;
        //}

        private SessionProvider() {
        }

        public static SessionProvider Instance {
            get {
                if (_instance == null) {
                    //_instance = new SessionProvider(Container.get<Assemblies>());
                }
                return _instance;
            }
        }

        public bool IsTestMode { get; set; }
        public bool IsBuildScheme { get; set; }
        protected bool IsBuildSchemeOnSessionCreated { get; set; }

        public ISessionFactory SessionFactory {
            get {
                if (session_factory == null) {
                    lock (lock_flag) {
                        if (session_factory == null) {
                            Initilize();
                        }
                    }
                }
                return session_factory;
            }
        }

        public ISession CurrentSession {
            get { return SessionFactory.GetCurrentSession(); }
        }

        public void Initilize() {
            if (!IsTestMode) {
                session_factory = Fluently.Configure()
                    .Cache(x =>
                        x.UseQueryCache().UseSecondLevelCache().UseMinimalPuts()
                        .ProviderClass<SysCacheProvider>()
                    )
                    .Database(
                        MsSqlConfiguration.MsSql2000
                            .ConnectionString(
                                ConfigurationManager.ConnectionStrings["Database"].ConnectionString))

                    .Mappings(m => assemblies.each(a => m.FluentMappings.AddFromAssembly(a)))
                    .ExposeConfiguration(c => c.SetProperty("current_session_context_class", "web"))
                    .ExposeConfiguration(build_schema)
                    .ExposeConfiguration(x => x.SetProperty("expiration", "900"))

                    .BuildSessionFactory();

            } else {
                session_factory = Fluently.Configure()
                    .Database(
                        MsSqlConfiguration.MsSql2000.ShowSql()
                            .ConnectionString(
                                ConfigurationManager.ConnectionStrings["Database"].ConnectionString))
                    .Mappings(m => assemblies.each(a => m.FluentMappings.AddFromAssembly(a)))
                    .ExposeConfiguration(c => c.SetProperty("current_session_context_class", "web"))
                    .ExposeConfiguration(build_schema)
                    .BuildSessionFactory();

            }

        }

        public void InitializeMemoryDBForTest() {
            IsBuildSchemeOnSessionCreated = true;
            session_factory = Fluently.Configure()
                .Database(SQLiteConfiguration.Standard.InMemory().ShowSql())
                .Mappings(m => assemblies.each(a => m.FluentMappings.AddFromAssembly(a)))
                .ExposeConfiguration(c => c.SetProperty("current_session_context_class", "thread_static"))
                .ExposeConfiguration(c => c.SetProperty("connection.driver_class", "NHibernate.Driver.SQLite20Driver"))
                .ExposeConfiguration(c => configuration = c)

                .BuildSessionFactory();

        }

        public void InitializeSQLFileDBForTest() {
            IsBuildSchemeOnSessionCreated = true;
            session_factory = Fluently.Configure()
                .Database(SQLiteConfiguration.Standard.UsingFile(@"C:\Temp\Database.sqlite").ShowSql())
                .Mappings(m => assemblies.each(a => m.FluentMappings.AddFromAssembly(a)))
                .ExposeConfiguration(c => c.SetProperty("current_session_context_class", "thread_static"))
                .ExposeConfiguration(c => c.SetProperty("connection.driver_class", "NHibernate.Driver.SQLite20Driver"))
                .ExposeConfiguration(c => configuration = c)
                .ExposeConfiguration(build_schema)
                .BuildSessionFactory();
        }

        public void InitializeSQLServerDBForTest() {
            session_factory = Fluently.Configure()
                   .Database(
                       MsSqlConfiguration.MsSql2000.ShowSql()
                           .ConnectionString(
                               ConfigurationManager.ConnectionStrings["Database"].ConnectionString))
                   .Mappings(m => assemblies.each(a => m.FluentMappings.AddFromAssembly(a)))
                   .ExposeConfiguration(c => c.SetProperty("current_session_context_class", "thread_static"))
                   .BuildSessionFactory();
        }


        public ISession CreateSession() {

            ISession session = SessionFactory.OpenSession();
            if (IsBuildSchemeOnSessionCreated) {
                var connection = session.Connection;
                new SchemaExport(configuration)
                    .Execute(true, true, false, connection, null);
            }
            return session;

        }

        private void build_schema(Configuration configuration) {
            if (IsBuildScheme) {
                new SchemaExport(configuration)
                    .Execute(IsTestMode, true, false);
            }
        }
    }
}