using System;
using System.Collections.Generic;

namespace Shopify.GraphQlResponse.linkShare
{
    public class GraphQlOrderResponseLinkShare
    {
        public Data data { get; set; }
        public Extensions extensions { get; set; }
    }

    public class Data
    {
        public Order order { get; set; }
    }

    public class Order
    {
        public DateTime createdAt { get; set; }
        public Customer customer { get; set; }
        public string email { get; set; }
        public string id { get; set; }
        public Metafields metafields { get; set; }
        public string name { get; set; }
    }

    public class Customer
    {
        public string firstName { get; set; }
        public string lastName { get; set; }
    }

    public class Metafields
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
        public string description { get; set; }
        public string id { get; set; }
        public string key { get; set; }
        public string legacyResourceId { get; set; }
        public string _namespace { get; set; }
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
