using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shopify.GraphQlResponse
{
    public class GraphQlOrderResponse
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
        public Pageinfo pageInfo { get; set; }
        public Edge[] edges { get; set; }
    }

    public class Pageinfo
    {
        public bool hasNextPage { get; set; }
        public bool hasPreviousPage { get; set; }
    }

    public class Edge
    {
        public string cursor { get; set; }
        public Node node { get; set; }
    }

    public class Node
    {
        public string id { get; set; }
        public string name { get; set; }
        public string createdAt { get; set; }
        public string updatedAt { get; set; }
        public Transaction[] transactions { get; set; }
        public Lineitems lineItems { get; set; }
        public Refund[] refunds { get; set; }
        public Totalpriceset totalPriceSet { get; set; }
        public Physicallocation physicalLocation { get; set; }
        public string displayFinancialStatus { get; set; }
        public string displayFulfillmentStatus { get; set; }
        public Shippingline shippingLine { get; set; }
    }
    public class Shippingline
    {
        public string title { get; set; }
        public Originalpriceset originalPriceSet { get; set; }
    }
    public class Originalpriceset
    {
        public Shopmoney shopMoney { get; set; }
    }

    public class Shopmoney
    {
        public string amount { get; set; }
    }

    public class Lineitems
    {
        public Edge1[] edges { get; set; }
    }

    public class Edge1
    {
        public Node1 node { get; set; }
    }

    public class Node1
    {
        public string id { get; set; }
        public string name { get; set; }
        public long quantity { get; set; }
        public string variantTitle { get; set; }
        public string sku { get; set; }
        public Originaltotalset originalTotalSet { get; set; }
        public List<Discountallocation> discountAllocations { get; set; }
        public Variant variant { get; set; }
        public string fulfillmentStatus { get; set; }
    }
   
    public class Originaltotalset
    {
        public Presentmentmoney presentmentMoney { get; set; }
    }

    public class Presentmentmoney
    {
        public string amount { get; set; }
    }

    public class Variant
    {
        public string barcode { get; set; }
    }
    public class Discountallocation
    {
        public Allocatedamountset allocatedAmountSet { get; set; }
    }
    public class Allocatedamountset
    {
        public Presentmentmoney presentmentMoney { get; set; }
    }
    public class Totalpriceset
    {
        public Presentmentmoney presentmentMoney { get; set; }
    }

    

    public class Physicallocation
    {
        public string id { get; set; }
    }

    public class Transaction
    {
        public DateTime createdAt { get; set; }
        public string kind { get; set; }
        public string gateway { get; set; }
        public Amountset amountSet { get; set; }
    }

    public class Amountset
    {
        public Presentmentmoney presentmentMoney { get; set; }
    }

   

    public class Refund
    {
        public Refundlineitems refundLineItems { get; set; }
        public Transactions transactions { get; set; }
    }

    public class Refundlineitems
    {
        public Edge2[] edges { get; set; }
    }

    public class Edge2
    {
        public Node2 node { get; set; }
    }

    public class Node2
    {
        public long quantity { get; set; }
        public Lineitem lineItem { get; set; }
    }

    public class Lineitem
    {
        public string id { get; set; }
        public string name { get; set; }
        public long quantity { get; set; }
    }

    public class Transactions
    {
        public Edge3[] edges { get; set; }
    }

    public class Edge3
    {
        public Node3 node { get; set; }
    }

    public class Node3
    {
        public DateTime createdAt { get; set; }
        public string kind { get; set; }
        public string gateway { get; set; }
        public Amountset amountSet { get; set; }
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