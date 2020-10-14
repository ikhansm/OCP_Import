using System;

namespace Shopify.Request
{

    public class VariantRootobject
    {
        public Variant variant { get; set; }
    }

    public class Variant
    {
        public long id { get; set; }
        public long product_id { get; set; }
        public string title { get; set; }
        public string price { get; set; }
        public string sku { get; set; }
        public int position { get; set; }
        public string inventory_policy { get; set; }
        public string compare_at_price { get; set; }
        public string fulfillment_service { get; set; }
        public string inventory_management { get; set; }
        public string option1 { get; set; }
        public string option2 { get; set; }
        public string option3 { get; set; }
        public DateTime created_at { get; set; }
        public DateTime updated_at { get; set; }
        public bool taxable { get; set; }
        public string barcode { get; set; }
        public int grams { get; set; }
        public object image_id { get; set; }
        public int inventory_quantity { get; set; }
        public float weight { get; set; }
        public string weight_unit { get; set; }
        public long inventory_item_id { get; set; }
        public int old_inventory_quantity { get; set; }
        public bool requires_shipping { get; set; }
        public string admin_graphql_api_id { get; set; }
        public Presentment_Prices[] presentment_prices { get; set; }
    }
    public class Presentment_Prices
    {
        public Price price { get; set; }
        public string compare_at_price { get; set; }
    }

    public class Price
    {
        public string currency_code { get; set; }
        public string amount { get; set; }
    }

}
