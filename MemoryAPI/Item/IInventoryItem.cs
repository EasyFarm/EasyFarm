namespace MemoryAPI
{
    public abstract class AbstractInventoryItem : IInventoryItem
    {
        public virtual uint Count { get; set; }
        public virtual ushort Extra { get; set; }
        public virtual uint Flag { get; set; }
        public virtual ushort ID { get; set; }
        public virtual byte Index { get; set; }
        public virtual InventoryType Location { get; set; }
        public virtual string Name { get; set; }
        public virtual uint Price { get; set; }
    }
}