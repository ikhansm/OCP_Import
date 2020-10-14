using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shopify.Response
{
    public class InventoryItemResponse
    {
        public Inventory_Item inventory_item { get; set; }
    }

    public class Inventory_Item
    {
        public long id { get; set; }
        public string sku { get; set; }
        public DateTime created_at { get; set; }
        public DateTime updated_at { get; set; }
        public string cost { get; set; }
        public bool tracked { get; set; }
        public string admin_graphql_api_id { get; set; }
    }

}
