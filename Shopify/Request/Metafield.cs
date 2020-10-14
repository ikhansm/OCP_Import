using Newtonsoft.Json;

namespace Shopify.Request
{
    public class Metafield
    {
        [RestSharp.Serializers.SerializeAs(Name = "namespace")]
        [JsonProperty(PropertyName = "namespace")]
        public string _namespace { get; set; }
        public string key { get; set; }
        public string value { get; set; }
        public string value_type { get; set; }
    }
}
