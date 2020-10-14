using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shopify.Response
{
    public class RootDiscount
    {
        public Discount_Code discount_code { get; set; }
    }

    public class Discount_Code
    {
        public long id { get; set; }
        public long price_rule_id { get; set; }
        public string code { get; set; }
        public int usage_count { get; set; }
        public DateTime created_at { get; set; }
        public DateTime updated_at { get; set; }
    }


    public class RootDiscount_Code_Creation
    {
        public Discount_Code_Creation discount_code_creation { get; set; }
    }

    public class Discount_Code_Creation
    {
        public long id { get; set; }
        public long price_rule_id { get; set; }
        public object started_at { get; set; }
        public object completed_at { get; set; }
        public DateTime created_at { get; set; }
        public DateTime updated_at { get; set; }
        public string status { get; set; }
        public int codes_count { get; set; }
        public int imported_count { get; set; }
        public int failed_count { get; set; }
    }
}
