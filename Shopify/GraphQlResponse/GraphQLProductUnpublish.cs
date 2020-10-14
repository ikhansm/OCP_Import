using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shopify.GraphQlResponse.UnPublish
{
    public class GraphQLProductUnpublish
    {
        public PrdUnpublishData data { get; set; }
        public Error[] errors { get; set; }
        public Extensions extensions { get; set; }
    }

    public class PrdUnpublishData
    {
        public object publishableUnpublish { get; set; }
    }

    public class Error
    {
        public string message { get; set; }
        public Location[] locations { get; set; }
        public string[] path { get; set; }
    }

    public class Location
    {
        public int line { get; set; }
        public int column { get; set; }
    }

}
