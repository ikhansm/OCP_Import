using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shopify.GraphQlResponse.CollectionProduct
{
    public class GraphQlProductIdResponse
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
        public Node node { get; set; }
        public string cursor { get; set; }
    }

    public class Node
    {
        public string id { get; set; }
        public string vendor { get; set; }
        public string[] tags { get; set; }
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
