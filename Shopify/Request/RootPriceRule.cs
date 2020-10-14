using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shopify.Request
{
    public class RootPriceRule
    {
        public Price_Rule price_rule { get; set; }
    }

    public class Price_Rule
    {
        public string title { get; set; }
        public string target_type { get; set; }
        public string target_selection { get; set; }
        public string allocation_method { get; set; }
        public string value_type { get; set; }
        public string value { get; set; }
        public string customer_selection { get; set; }
        public List<long> entitled_collection_ids { get; set; }
        public DateTime starts_at { get; set; }
        public DateTime ends_at { get; set; }
        public string usage_limit { get; set; }
    }



    public class RootobjectPriceTemp
    {
        public Price_RuleTemp price_rule { get; set; }
    }

    public class Price_RuleTemp
    {
        public string title { get; set; }
        public string target_type { get; set; }
        public string target_selection { get; set; }
        public string allocation_method { get; set; }
        public string value_type { get; set; }
        public string value { get; set; }
        public string customer_selection { get; set; }
        public List<long> entitled_collection_ids { get; set; }
        public DateTime starts_at { get; set; }
    }



    public class RootobjectSmartCollection
    {
        public Smart_Collection smart_collection { get; set; }
    }

    public class Smart_Collection
    {
        public string id { get; set; }
        public List<Rule> rules { get; set; }
    }

    public class Rule
    {
        public string column { get; set; }
        public string relation { get; set; }
        public string condition { get; set; }
    }
}
