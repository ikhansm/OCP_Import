using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shopify.GraphQlResponse.Inventory
{
    public class GraphQlInventoryItemIdByUPC
    {
        public Data data { get; set; }
        public Extensions extensions { get; set; }
    }

    public class Data
    {
        public Productvariants productVariants { get; set; }
    }

    public class Productvariants
    {
        public List<Edge> edges { get; set; }
    }

    public class Edge
    {
        public string cursor { get; set; }
        public Node node { get; set; }
    }

    public class Node
    {
        public string id { get; set; }
        public Inventoryitem inventoryItem { get; set; }
    }

    public class Inventoryitem
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
