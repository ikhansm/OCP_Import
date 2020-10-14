using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shopify.Request.variants
{
    public class ProductVariantMetafieldRequest
    {
        public Metafield metafield { get; set; }
    }

    public class Metafield
    {
        public string _namespace { get; set; }
        public string key { get; set; }
        public string value { get; set; }
        public string value_type { get; set; }
    }
}
