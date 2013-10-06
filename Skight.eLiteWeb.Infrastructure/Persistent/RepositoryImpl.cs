using System.Collections.Generic;
using NHibernate;
using Skight.eLiteWeb.Domain.Containers;
using Skight.eLiteWeb.Domain.Persistent;

namespace Skight.eLiteWeb.Infrastructure.Persistent
{
     [RegisterInContainer(LifeCycle.singleton)]
    public class RepositoryImpl : Repository
     {
         public IEnumerable<Item> get_all_items_matching<Item>(Query<Item> query)
         {
             throw new System.NotImplementedException();
         }

         public Item get_single_item_matching<Item>(Query<Item> query)
         {
             throw new System.NotImplementedException();
         }

         public bool is_existed<Item>(Query<Item> query)
         {
             throw new System.NotImplementedException();
         }

         public void save<Item>(Item item)
         {
             session.Save(item);
         }

         public Item get_by_id<Item>(int id)
         {
            return session.Get<Item>(id);
         }

         public void delete<Item>(Item item)
         {
             session.Delete(item);
         }
         private ISession session {
             get { return SessionProvider.Instance.CurrentSession; }
         }
     }
}
