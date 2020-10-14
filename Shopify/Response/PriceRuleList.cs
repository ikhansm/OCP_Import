using System;
using System.Collections.Generic;

namespace Shopify.Response
{
    public class PriceRuleListViewModel
    {
        public List<Price_Rules> price_rules { get; set; }
    }

    public class Price_Rules
    {
        public long id { get; set; }
        public string value_type { get; set; }
        public string value { get; set; }
        public string customer_selection { get; set; }
        public string target_type { get; set; }
        public string target_selection { get; set; }
        public string allocation_method { get; set; }
        public object allocation_limit { get; set; }
        public bool once_per_customer { get; set; }
        public string usage_limit { get; set; }
        public DateTime starts_at { get; set; }
        public DateTime? ends_at { get; set; }
        public DateTime created_at { get; set; }
        public DateTime updated_at { get; set; }
        public List<long> entitled_product_ids { get; set; }
        public List<long> entitled_variant_ids { get; set; }
        public List<long> entitled_collection_ids { get; set; }
        public List<long> entitled_country_ids { get; set; }
        public List<long> prerequisite_product_ids { get; set; }
        public List<long> prerequisite_variant_ids { get; set; }
        public List<long> prerequisite_collection_ids { get; set; }
        public List<long> prerequisite_saved_search_ids { get; set; }
        public List<long> prerequisite_customer_ids { get; set; }
        public Prerequisite_To_Entitlement_Quantity_Ratio prerequisite_to_entitlement_quantity_ratio { get; set; }
        public string title { get; set; }
        public string admin_graphql_api_id { get; set; }
    }
    

}
