namespace MemoryAPI
{
    public interface IInventoryItem
    {
        uint Count { get; set; }
        ushort Extra { get; set; }
        uint Flag { get; set; }
        ushort ID { get; set; }
        byte Index { get; set; }
        InventoryType Location { get; set; }
        string Name { get; set; }
        uint Price { get; set; }
    }
}