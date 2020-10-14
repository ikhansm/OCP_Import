using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shopify.Response
{


    public class GetInventoryLevel
    {
        public List<Inventory_Levels> inventory_levels { get; set; }
    }

    public class Inventory_Levels
    {
        public long inventory_item_id { get; set; }
        public long? location_id { get; set; }
        public int? available { get; set; }
        public DateTime updated_at { get; set; }
        public string admin_graphql_api_id { get; set; }
    }


    public class InventoryLevelResponse
    {
        public Inventory_Level inventory_level { get; set; }
        public List<Inventory_Levels> inventory_levels { get; set; }
    }

    public class Inventory_Level
    {
        public long inventory_item_id { get; set; }
        public long location_id { get; set; }
        public int? available { get; set; }
        public DateTime? updated_at { get; set; }
        public string admin_graphql_api_id { get; set; }
    }
}
