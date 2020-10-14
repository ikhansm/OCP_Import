namespace Shopify.Request
{
    public class InventoryLevel
    {
        public long location_id { get; set; }
        public long inventory_item_id { get; set; }
        public int available { get; set; }
    }

}
