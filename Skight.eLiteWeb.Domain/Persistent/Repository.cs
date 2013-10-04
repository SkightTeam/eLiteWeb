using System.Collections.Generic;

namespace Skight.eLiteWeb.Domain.Persistent
{
    public interface Repository
    {
        IEnumerable<Item> get_all_items_matching<Item>(Query<Item> query);
        Item get_single_item_matching<Item>(Query<Item> query);
        bool is_existed<Item>(Query<Item> query);
        void save<Item>(Item item);
        Item get_by_id<Item>(int id);
        void delete<Item>(Item item);
    }
}