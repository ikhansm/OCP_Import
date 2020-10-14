using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shopify.Response
{
    public class TotalOrderCount
    {
        public int count { get; set; }
    }

    public class ShopifyOrders
    {
        public List<Order> orders { get; set; }
    }

    public class OrderResponse
    {
        public Order order { get; set; }
    }

    public class CancelOrderResponse
    {
        public string notice { get; set; }
        public Order order { get; set; }

        public string errors { get; set; }
    }

    public class Order
    {
        public long id { get; set; }
        public string email { get; set; }
        public string closed_at { get; set; }
        public string created_at { get; set; }
        public string updated_at { get; set; }
        public int number { get; set; }
        public string note { get; set; }
        public string token { get; set; }
        public string gateway { get; set; }
        public bool test { get; set; }
        public string total_price { get; set; }
        public string subtotal_price { get; set; }
        public int total_weight { get; set; }
        public string total_tax { get; set; }
        public bool taxes_included { get; set; }
        public string currency { get; set; }
        public string financial_status { get; set; }
        public bool confirmed { get; set; }
        public string total_discounts { get; set; }
        public string total_line_items_price { get; set; }
        public string cart_token { get; set; }
        public bool buyer_accepts_marketing { get; set; }
        public string name { get; set; }
        public string referring_site { get; set; }
        public string landing_site { get; set; }
        public string cancelled_at { get; set; }
        public string cancel_reason { get; set; }
        public string total_price_usd { get; set; }
        public string checkout_token { get; set; }
        public string reference { get; set; }
        public string user_id { get; set; }
        public Nullable<long> location_id { get; set; }
        public string source_identifier { get; set; }
        public string source_url { get; set; }
        public string processed_at { get; set; }
        public string device_id { get; set; }
        public string phone { get; set; }
        public string customer_locale { get; set; }
        public int app_id { get; set; }
        public string browser_ip { get; set; }
        public string landing_site_ref { get; set; }
        public int order_number { get; set; }
        public List<DiscountApplication> discount_applications { get; set; }
        public List<DiscountCode> discount_codes { get; set; }
        public List<object> note_attributes { get; set; }
        public List<string> payment_gateway_names { get; set; }
        public string processing_method { get; set; }
        public string checkout_id { get; set; }
        public string source_name { get; set; }
        public string fulfillment_status { get; set; }
        public List<TaxLine> tax_lines { get; set; }
        public string tags { get; set; }
        public string contact_email { get; set; }
        public string order_status_url { get; set; }
        public string admin_graphql_api_id { get; set; }
        public List<Line_Item> line_items { get; set; }
        public List<ShippingLine> shipping_lines { get; set; }
        public BillingAddress billing_address { get; set; }
        public ShippingAddress shipping_address { get; set; }
        public List<Fulfillment> fulfillments { get; set; }
        public ClientDetails client_details { get; set; }
        public List<Refund> refunds { get; set; }
        public PaymentDetails payment_details { get; set; }
        public Shopify.Response.Customer customer { get; set; }
    }
    public class DiscountCode
    {
        public string code { get; set; }
        public string amount { get; set; }
        public string type { get; set; }
    }
    public class DiscountApplication
    {
        public string type { get; set; }
        public string value { get; set; }
        public string value_type { get; set; }
        public string allocation_method { get; set; }
        public string target_selection { get; set; }
        public string target_type { get; set; }
        public string description { get; set; }
    }

    public class Fulfillment
    {
        public long id { get; set; }
        public long order_id { get; set; }
        public string status { get; set; }
        public string created_at { get; set; }
        public string service { get; set; }
        public string updated_at { get; set; }
        public string tracking_company { get; set; }
        public string shipment_status { get; set; }
        public long location_id { get; set; }
        public string tracking_number { get; set; }
        public List<string> tracking_numbers { get; set; }
        public string tracking_url { get; set; }
        public List<string> tracking_urls { get; set; }
        public Receipt receipt { get; set; }
        public string name { get; set; }
        public List<Line_Item> line_items { get; set; }
    }
    public class ClientDetails
    {
        public string browser_ip { get; set; }
        public object accept_language { get; set; }
        public string user_agent { get; set; }
        public object session_hash { get; set; }
        public object browser_width { get; set; }
        public object browser_height { get; set; }
    }

    public class ShippingAddress
    {
        public string first_name { get; set; }
        public string address1 { get; set; }
        public string phone { get; set; }
        public string city { get; set; }
        public string zip { get; set; }
        public string province { get; set; }
        public string country { get; set; }
        public string last_name { get; set; }
        public object address2 { get; set; }
        public object company { get; set; }
        public Nullable<double> latitude { get; set; }
        public Nullable<double> longitude { get; set; }
        public string name { get; set; }
        public string country_code { get; set; }
        public string province_code { get; set; }
    }

    public class BillingAddress
    {
        public string first_name { get; set; }
        public string address1 { get; set; }
        public string phone { get; set; }
        public string city { get; set; }
        public string zip { get; set; }
        public string province { get; set; }
        public string country { get; set; }
        public string last_name { get; set; }
        public object address2 { get; set; }
        public object company { get; set; }
        public Nullable<double> latitude { get; set; }
        public Nullable<double> longitude { get; set; }
        public string name { get; set; }
        public string country_code { get; set; }
        public string province_code { get; set; }
    }

    public class ShippingLine
    {
        public long id { get; set; }
        public string title { get; set; }
        public string price { get; set; }
        public string code { get; set; }
        public string source { get; set; }
        public string phone { get; set; }
        public object requested_fulfillment_service_id { get; set; }
        public object delivery_category { get; set; }
        public object carrier_identifier { get; set; }
        public string discounted_price { get; set; }
        public List<DiscountAllocation> discount_allocations { get; set; }
        public List<TaxLine> tax_lines { get; set; }
    }

    public class DiscountAllocation
    {
        public string amount { get; set; }
        public Nullable<int> discount_application_index { get; set; }
    }

    public class Refund
    {
        public long id { get; set; }
        public long order_id { get; set; }
        public string created_at { get; set; }
        public string note { get; set; }
        public Nullable<long> user_id { get; set; }
        public string processed_at { get; set; }
        public bool restock { get; set; }
        public List<Refund_Line_Items> refund_line_items { get; set; }
        public List<Transaction> transactions { get; set; }
        public List<Order_Adjustments> order_adjustments { get; set; }
    }

    public class Refund_Line_Items
    {
        public long id { get; set; }
        public int quantity { get; set; }
        public long line_item_id { get; set; }
        public Nullable<long> location_id { get; set; }
        public string restock_type { get; set; }
        public float subtotal { get; set; }
        public float total_tax { get; set; }
        public Line_Item line_item { get; set; }
    }

    public class Order_Adjustments
    {
        public long id { get; set; }
        public long order_id { get; set; }
        public long refund_id { get; set; }
        public string amount { get; set; }
        public string tax_amount { get; set; }
        public string kind { get; set; }
        public string reason { get; set; }
    }

    public class Line_Item
    {
        public long id { get; set; }
        public long? variant_id { get; set; }
        public string title { get; set; }
        public int quantity { get; set; }
        public string price { get; set; }
        public string sku { get; set; }
        public string variant_title { get; set; }
        public string vendor { get; set; }
        public string fulfillment_service { get; set; }
        public Nullable<long> product_id { get; set; }
        public bool requires_shipping { get; set; }
        public bool taxable { get; set; }
        public bool gift_card { get; set; }
        public string tax_code { get; set; }
        public string pre_tax_price { get; set; }
        public string name { get; set; }
        public string variant_inventory_management { get; set; }
        public List<Property> properties { get; set; }
        public bool product_exists { get; set; }
        public int fulfillable_quantity { get; set; }
        public int grams { get; set; }
        public string total_discount { get; set; }
        public string fulfillment_status { get; set; }
        public List<DiscountAllocation> discount_allocations { get; set; }
        public string admin_graphql_api_id { get; set; }
        public List<TaxLine2> tax_lines { get; set; }
        public OriginLocation origin_location { get; set; }
        public DestinationLocation destination_location { get; set; }
    }
    public class Property
    {
        public string name { get; set; }
        public string value { get; set; }
    }
    public class OriginLocation
    {
        public long id { get; set; }
        public string country_code { get; set; }
        public string province_code { get; set; }
        public string name { get; set; }
        public string address1 { get; set; }
        public string address2 { get; set; }
        public string city { get; set; }
        public string zip { get; set; }
    }

    public class PaymentDetails
    {
        public string credit_card_bin { get; set; }
        public string avs_result_code { get; set; }
        public string cvv_result_code { get; set; }
        public string credit_card_number { get; set; }
        public string credit_card_company { get; set; }
    }

    public class TaxLine
    {
        public string price { get; set; }
        public double rate { get; set; }
        public string title { get; set; }
    }
    public class TaxLine2
    {
        public string title { get; set; }
        public string price { get; set; }
        public double rate { get; set; }
    }

    public class DestinationLocation
    {
        public long id { get; set; }
        public string country_code { get; set; }
        public string province_code { get; set; }
        public string name { get; set; }
        public string address1 { get; set; }
        public string address2 { get; set; }
        public string city { get; set; }
        public string zip { get; set; }
    }

    public class TaxLine3
    {
        public string title { get; set; }
        public string price { get; set; }
        public double rate { get; set; }
    }

    public class RootErrors
    {
        public string errors { get; set; }
    }
}








