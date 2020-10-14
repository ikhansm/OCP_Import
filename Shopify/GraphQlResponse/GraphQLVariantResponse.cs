using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shopify.GraphQlResponse.MexicoCost
{
    public class GraphQLVariantResponse
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
        public Edge[] edges { get; set; }
    }

    public class Edge
    {
        public Node node { get; set; }
    }

    public class Node
    {
        public string id { get; set; }
        public string barcode { get; set; }
        public string sku { get; set; }
        public Inventoryitem inventoryItem { get; set; }
    }

    public class Inventoryitem
    {
        public string id { get; set; }
        public Unitcost unitCost { get; set; }
    }

    public class Unitcost
    {
        public string amount { get; set; }
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
