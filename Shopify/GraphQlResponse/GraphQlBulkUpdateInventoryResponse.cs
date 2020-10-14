using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shopify.GraphQlResponse.BulkInventoryUpdate
{
    public class GraphQlBulkUpdateInventoryResponse
    {
        public Data data { get; set; }
        public Extensions extensions { get; set; }
    }

    public class Data
    {
        public Inventorybulkadjustquantityatlocation inventoryBulkAdjustQuantityAtLocation { get; set; }
    }

    public class Inventorybulkadjustquantityatlocation
    {
        public Inventorylevel[] inventoryLevels { get; set; }
        public List<Usererror> userErrors { get; set; }
    }

    public class Usererror
    {
        public string[] field { get; set; }
        public string message { get; set; }
    }
//    inventoryLevels {
//      available
//      id
//      item {
//        id
//}
//location {
//        id
//      }
//    }
    public class Inventorylevel
    {
        public long available { get; set; }
        public string id { get; set; }
        public Item item { get; set; }
        public Location location { get; set; }
    }

    public class Item
    {
        public string id { get; set; }
    }

    public class Location
    {
        public string id { get; set; }
    }

    public class Extensions
    {
        public Cost cost { get; set; }
    }

    public class Cost
    {
        public long requestedQueryCost { get; set; }
        public long actualQueryCost { get; set; }
        public Throttlestatus throttleStatus { get; set; }
    }

    public class Throttlestatus
    {
        public long maximumAvailable { get; set; }
        public long currentlyAvailable { get; set; }
        public long restoreRate { get; set; }
    }
}
