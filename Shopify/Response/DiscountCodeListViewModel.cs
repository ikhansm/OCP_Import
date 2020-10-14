using System;
using System.Collections.Generic;

namespace Shopify.Response
{
    public class DiscountCodeListViewModel
    {
        public List<Discount_Codes> discount_codes { get; set; }
    }

    public class Discount_Codes
    {
        public long id { get; set; }
        public long price_rule_id { get; set; }
        public string code { get; set; }
        public int usage_count { get; set; }
        public DateTime created_at { get; set; }
        public DateTime updated_at { get; set; }
    }

}
