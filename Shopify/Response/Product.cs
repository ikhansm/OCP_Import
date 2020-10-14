using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shopify.Response
{



    public class Productobject
    {
        public List<Product> products { get; set; }
        public Product product { get; set; }
    }

    public class Product
    {
        public long id { get; set; }
        public string title { get; set; }
        public string body_html { get; set; }
        public string vendor { get; set; }
        public string product_type { get; set; }
        public DateTime? created_at { get; set; }
        public string handle { get; set; }
        public DateTime? updated_at { get; set; }
        public DateTime? published_at { get; set; }
        public string template_suffix { get; set; }
        public string tags { get; set; }
        public string published_scope { get; set; }
        public string admin_graphql_api_id { get; set; }
        public List<Variant> variants { get; set; }
        public Option[] options { get; set; }
        public Image1[] images { get; set; }
        public Image image { get; set; }
    }

    public class Image
    {
        public long id { get; set; }
        public long product_id { get; set; }
        public long position { get; set; }
        public DateTime? created_at { get; set; }
        public DateTime? updated_at { get; set; }
        public object alt { get; set; }
        public long width { get; set; }
        public long height { get; set; }
        public string src { get; set; }
        public object[] variant_ids { get; set; }
        public string admin_graphql_api_id { get; set; }
    }

    public class Variant
    {
        public long id { get; set; }
        public long product_id { get; set; }
        public string title { get; set; }
        public string price { get; set; }
        public string sku { get; set; }
        public long position { get; set; }
        public string inventory_policy { get; set; }
        public string compare_at_price { get; set; }
        public string fulfillment_service { get; set; }
        public string inventory_management { get; set; }
        public string option1 { get; set; }
        public string option2 { get; set; }
        public string option3 { get; set; }
        public DateTime? created_at { get; set; }
        public DateTime? updated_at { get; set; }
        public bool taxable { get; set; }
        public string barcode { get; set; }
        public long grams { get; set; }
        public long? image_id { get; set; }
        public long inventory_quantity { get; set; }
        public float weight { get; set; }
        public string weight_unit { get; set; }
        public long? inventory_item_id { get; set; }
        public long old_inventory_quantity { get; set; }
        public bool requires_shipping { get; set; }
        public string admin_graphql_api_id { get; set; }
    }

    public class Option
    {
        public long id { get; set; }
        public long product_id { get; set; }
        public string name { get; set; }
        public long position { get; set; }
        public string[] values { get; set; }
    }

    public class Image1
    {
        public long id { get; set; }
        public long product_id { get; set; }
        public long position { get; set; }
        public DateTime? created_at { get; set; }
        public DateTime? updated_at { get; set; }
        public string alt { get; set; }
        public long width { get; set; }
        public long height { get; set; }
        public string src { get; set; }
        public long?[] variant_ids { get; set; }
        public string admin_graphql_api_id { get; set; }
    }

}
