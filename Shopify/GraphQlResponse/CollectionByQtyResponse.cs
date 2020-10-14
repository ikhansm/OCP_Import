using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shopify.GraphQlResponse.Collection
{

    public class CollectionByQtyResponse
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
        public Pageinfo pageInfo { get; set; }
    }

    public class Pageinfo
    {
        public bool hasNextPage { get; set; }
    }

    public class Edge
    {
        public Node node { get; set; }
        public string cursor { get; set; }
    }

    public class Node
    {
        public Inventoryitem inventoryItem { get; set; }
        public Product product { get; set; }
    }

    public class Inventoryitem
    {
        public string id { get; set; }
        public Inventorylevels inventoryLevels { get; set; }
    }

    public class Inventorylevels
    {
        public Edge1[] edges { get; set; }
    }

    public class Edge1
    {
        public Node1 node { get; set; }
    }

    public class Node1
    {
        public long available { get; set; }
        public Location location { get; set; }
    }

    public class Location
    {
        public string id { get; set; }
    }

    public class Product
    {
        public string id { get; set; }
        public string title { get; set; }
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
