using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shopify.Request
{

    public class InventoryItemRootobject
    {
        public Inventory_Item inventory_item { get; set; }
    }

    public class Inventory_Item
    {
        public long id { get; set; }
        public string cost { get; set; }
    }

}
