using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shopify.Response
{
    public class DiscountVariantIds
    {
        public List<long> VariantIds { get; set; }
        public int? MinQty { get; set; }
        public int status { get; set; } = 0;
    }

    public class RootPriceRule
    {
        public Price_Rule price_rule { get; set; }
    }

    public class PriceRuleResponse
    {
        public Price_Rule price_rule { get; set; }
    }

    public class Price_Rule
    {
        public long id { get; set; }
        public string value_type { get; set; }
        public string value { get; set; }
        public string customer_selection { get; set; }
        public string target_type { get; set; }
        public string target_selection { get; set; }
        public string allocation_method { get; set; }
        public bool once_per_customer { get; set; }
        public object usage_limit { get; set; }
        public DateTime? starts_at { get; set; }
        public object ends_at { get; set; }
        public DateTime? created_at { get; set; }
        public DateTime? updated_at { get; set; }
        public List<long> entitled_product_ids { get; set; }
        public List<long> entitled_variant_ids { get; set; }
        public List<long> entitled_collection_ids { get; set; }
        public List<long> entitled_country_ids { get; set; }
        public List<long> prerequisite_product_ids { get; set; }
        public List<long> prerequisite_variant_ids { get; set; }
        public List<long> prerequisite_collection_ids { get; set; }
        public List<long> prerequisite_saved_search_ids { get; set; }
        public List<long> prerequisite_customer_ids { get; set; }
        public PrerequisiteSubtotalRange prerequisite_subtotal_range { get; set; }
        public PrerequisiteQuantityRange prerequisite_quantity_range { get; set; }
        public object prerequisite_shipping_price_range { get; set; }
        public Prerequisite_To_Entitlement_Quantity_Ratio prerequisite_to_entitlement_quantity_ratio { get; set; }
        public string title { get; set; }
        //public string admin_graphql_api_id { get; set; }
    }

    public class PrerequisiteQuantityRange
    {
        public int? greater_than_or_equal_to { get; set; }
    }
    public class PrerequisiteSubtotalRange
    {
        public string greater_than_or_equal_to { get; set; }
    }

    public class Prerequisite_To_Entitlement_Quantity_Ratio
    {
        public object prerequisite_quantity { get; set; }
        public object entitled_quantity { get; set; }
    }
}
