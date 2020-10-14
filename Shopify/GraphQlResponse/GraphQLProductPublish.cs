using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shopify.GraphQlResponse.Publish
{
    public class GraphQLProductPublish
    {
        public prdData data { get; set; }
        public Extensions extensions { get; set; }
    }

    public class prdData
    {
        public Publishablepublish publishablePublish { get; set; }
    }

    public class Publishablepublish
    {
        public Publishable publishable { get; set; }
    }

    public class Publishable
    {
        public string id { get; set; }
        public string title { get; set; }
    }

}

