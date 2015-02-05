using System.Data;
using NHibernate;
using NHibernate.Context;
using Skight.eLiteWeb.Infrastructure.Persistent;
using Skight.eLiteWeb.Presentation.Web.FrontControllers;

namespace Skight.eLiteWeb.Application.CommandDecorators
{
    public class TransactionDecorator : DiscreteCommand
    {
        private readonly DiscreteCommand internal_command;

        public TransactionDecorator(DiscreteCommand internalCommand) 
        {
            internal_command = internalCommand;
        }

        #region DiscreteWebRequestCommand Membersn

        public void process(WebRequest request) 
        {
            using (ISession session = SessionProvider.Instance.CreateSession())
            {
                using (ITransaction transaction = session.BeginTransaction(SessionProvider.Instance.IsolationLevel))
                {
                    CurrentSessionContext.Bind(session);
                    internal_command.process(request);
                    CurrentSessionContext.Unbind(
                        SessionProvider.Instance.SessionFactory);
                    transaction.Commit();
                }
            }
        }

        #endregion
    }
}