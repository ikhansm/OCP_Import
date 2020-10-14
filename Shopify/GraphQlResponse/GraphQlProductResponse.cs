using System.Collections.Generic;

namespace Shopify.GraphQlResponse.Product
{


    public class GraphQlProductResponse
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
        public Pageinfo pageInfo { get; set; }
        public List<Edge> edges { get; set; }
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
        public List<string> tags { get; set; }
        public string title { get; set; }
        public string handle { get; set; }
        public string vendor { get; set; }
        public List<Option> options { get; set; }
    }

    public class Option
    {
        public List<string> values { get; set; }
        public string name { get; set; }
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
