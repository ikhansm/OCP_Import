using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shopify.Request
{
    public class FulfillmentRequest
    {
        public Fulfillment fulfillment { get; set; }
    }

    public class Fulfillment
    {
        public long location_id { get; set; }

        public bool notify_customer { get; set; }

        public List<Line_Items> line_items { get; set; }
    }

    public class Line_Items
    {
        public long id { get; set; }
        public int quantity { get; set; }
    }




}
