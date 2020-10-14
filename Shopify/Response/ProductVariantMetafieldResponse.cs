using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shopify.Response
{


    public class ProductVariantMetafieldResponse
    {
        public Metafield metafield { get; set; }
    }

    public class Metafield
    {
        public int id { get; set; }
        public string _namespace { get; set; }
        public string key { get; set; }
        public int value { get; set; }
        public string value_type { get; set; }
        public object description { get; set; }
        public int owner_id { get; set; }
        public DateTime created_at { get; set; }
        public DateTime updated_at { get; set; }
        public string owner_resource { get; set; }
        public string admin_graphql_api_id { get; set; }
    }


    

}
