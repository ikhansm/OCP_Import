using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shopify.GraphQlResponse.PreOrder
{
    public class GraphQlPreOrderProductVariantsResponse
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
        public string cursor { get; set; }
        public Node node { get; set; }
    }

    public class Node
    {
        public string barcode { get; set; }
        public string id { get; set; }
        public string inventoryPolicy { get; set; }
        public Metafields metafields { get; set; }
        public Selectedoption[] selectedOptions { get; set; }
        public Product product { get; set; }
    }

    public class Metafields
    {
        public Edge1[] edges { get; set; }
        public Pageinfo pageInfo { get; set; }
    }

    public class Pageinfo
    {
        public bool hasNextPage { get; set; }
    }

    public class Edge1
    {
        public string cursor { get; set; }
        public Node1 node { get; set; }
    }

    public class Node1
    {
        public string id { get; set; }
        public string key { get; set; }
        public string _namespace { get; set; }
        public string value { get; set; }
        public string valueType { get; set; }
        public object description { get; set; }
        public string legacyResourceId { get; set; }
    }

    public class Product
    {
        public string id { get; set; }
        public string handle { get; set; }
    }

    public class Selectedoption
    {
        public string name { get; set; }
        public string value { get; set; }
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
