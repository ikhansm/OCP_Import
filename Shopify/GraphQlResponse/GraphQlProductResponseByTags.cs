using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shopify.GraphQlResponse.SoldOut
{
    public class GraphQlProductResponseByTags
    {
        public Data data { get; set; }
        public Extensions extensions { get; set; }
    }

    public class Data
    {
        public Products products { get; set; }
    }

    public class Products
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
        public string cursor { get; set; }
        public Node node { get; set; }
    }

    public class Node
    {
        public string id { get; set; }
        public string handle { get; set; }
        public string[] tags { get; set; }
        public string title { get; set; }
        public string vendor { get; set; }
        public string totalInventory { get; set; }
        public string totalVariants { get; set; }
        public bool tracksInventory { get; set; }
        public Variants variants { get; set; }
        
    }

    public class Variants
    {
        public Edge1[] edges { get; set; }
    }

    public class Edge1
    {
        public string cursor { get; set; }
        public Node1 node { get; set; }
    }

    public class Node1
    {
        public string barcode { get; set; }
        public string id { get; set; }
        public string inventoryPolicy { get; set; }
        public string inventoryQuantity { get; set; }
        public Inventoryitem inventoryItem { get; set; }
        public string sku { get; set; }
    }

    public class Inventoryitem
    {
        public string id { get; set; }
        public Inventorylevels inventoryLevels { get; set; }
        public bool tracked { get; set; }
    }

    public class Inventorylevels
    {
        public Edge2[] edges { get; set; }
    }

    public class Edge2
    {
        public string cursor { get; set; }
        public Node2 node { get; set; }
    }

    public class Node2
    {
        public string available { get; set; }
        public string id { get; set; }
        public Location location { get; set; }
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
        public string requestedQueryCost { get; set; }
        public string actualQueryCost { get; set; }
        public Throttlestatus throttleStatus { get; set; }
    }

    public class Throttlestatus
    {
        public string maximumAvailable { get; set; }
        public string currentlyAvailable { get; set; }
        public string restoreRate { get; set; }
    }

}
