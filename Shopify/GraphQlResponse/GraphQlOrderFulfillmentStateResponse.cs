using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shopify.GraphQlResponse.ShippingState
{
    public class GraphQlOrderFulfillmentStateResponse
    {
        public Data data { get; set; }
        public Extensions extensions { get; set; }
    }


    public class Data
    {
        public Orders orders { get; set; }
    }

    public class Orders
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
        public Fulfillment[] fulfillments { get; set; }
        public string cancelReason { get; set; }
        public string email { get; set; }
        public string name { get; set; }
        public bool hasTimelineComment { get; set; }
        public bool confirmed { get; set; }
        public Customer customer { get; set; }
        public string displayFulfillmentStatus { get; set; }
    }

    public class Customer
    {
        public string firstName { get; set; }
        public string lastName { get; set; }
    }

    public class Fulfillment
    {
        public string status { get; set; }
        public DateTime? inTransitAt { get; set; }
        public string displayStatus { get; set; }
        public DateTime? deliveredAt { get; set; }
        public DateTime createdAt { get; set; }
        public DateTime? estimatedDeliveryAt { get; set; }
        public int totalQuantity { get; set; }
        public Trackinginfo[] trackingInfo { get; set; }
    }

    public class Trackinginfo
    {
        public string number { get; set; }
        public string company { get; set; }
        public string url { get; set; }
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
