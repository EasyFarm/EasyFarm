namespace MemoryAPI
{
    public abstract class AbstractItemTools
    {
        public virtual short CaseCount { get; }
        public virtual short CaseMax { get; }
        public virtual uint CurrentGil { get; }
        public virtual short InventoryCount { get; }
        public virtual short InventoryMax { get; }
        public virtual short LockerCount { get; }
        public virtual short LockerMax { get; }
        public virtual short SackCount { get; }
        public virtual short SackMax { get; }
        public virtual short SafeCount { get; }
        public virtual short SafeMax { get; }
        public virtual short SatchelCount { get; }
        public virtual short SatchelMax { get; }
        public virtual int SelectedItemID { get; }
        public virtual short SelectedItemIndex { get; }
        public virtual string SelectedItemName { get; }
        public virtual int SelectedItemNum { get; }
        public virtual short StorageCount { get; }
        public virtual short StorageMax { get; }
        public virtual short TemporaryCount { get; }
        public virtual short TemporaryMax { get; }
        public virtual short WardrobeCount { get; }
        public virtual short WardrobeMax { get; }

        public abstract IInventoryItem GetCaseItem(int index);
        public abstract uint GetCaseItemCount(ushort iD);
        public abstract uint GetCaseItemCountByIndex(byte index);
        public abstract int GetCaseItemIDByIndex(byte index);
        public abstract IInventoryItem GetEquippedItem(EquipSlot slot);
        public abstract uint GetEquippedItemCount(EquipSlot slot);
        public abstract int GetEquippedItemID(EquipSlot slot);
        public abstract byte GetEquippedItemIndex(EquipSlot slot);
        public abstract byte GetEquippedItemLocation(EquipSlot slot);
        public abstract int GetFirstIndexByItemID(int ID, InventoryType location);
        public abstract IInventoryItem GetInventoryItem(int index);
        public abstract uint GetInventoryItemCount(ushort iD);
        public abstract uint GetInventoryItemCountByIndex(int index);
        public abstract int GetInventoryItemIDByIndex(byte index);
        public abstract IInventoryItem GetItem(int index, InventoryType location);
        public abstract uint GetItemCount(int iD, InventoryType location);
        public abstract uint GetItemCountByIndex(int index, InventoryType location);
        public abstract int GetItemIDByIndex(int index, InventoryType location);
        public abstract System.Collections.Generic.List<IInventoryItem> GetItemList(int iD, InventoryType location);
        public abstract IInventoryItem GetLockerItem(int index);
        public abstract uint GetLockerItemCount(ushort iD);
        public abstract uint GetLockerItemCountByIndex(byte index);
        public abstract int GetLockerItemIDByIndex(byte index);
        public abstract IInventoryItem GetSackItem(int index);
        public abstract uint GetSackItemCount(ushort iD);
        public abstract uint GetSackItemCountByIndex(byte index);
        public abstract int GetSackItemIDByIndex(byte index);
        public abstract IInventoryItem GetSafeItem(int index);
        public abstract uint GetSafeItemCount(ushort iD);
        public abstract uint GetSafeItemCountByIndex(byte index);
        public abstract int GetSafeItemIDByIndex(byte index);
        public abstract IInventoryItem GetSatchelItem(int index);
        public abstract uint GetSatchelItemCount(ushort iD);
        public abstract uint GetSatchelItemCountByIndex(byte index);
        public abstract int GetSatchelItemIDByIndex(byte index);
        public abstract IInventoryItem GetStorageItem(int index);
        public abstract uint GetStorageItemCount(ushort iD);
        public abstract uint GetStorageItemCountByIndex(byte index);
        public abstract int GetStorageItemIDByIndex(byte index);
        public abstract IInventoryItem GetTempItem(int index);
        public abstract uint GetTempItemCount(ushort iD);
        public abstract uint GetTempItemCountByIndex(byte index);
        public abstract int GetTempItemIDByIndex(byte index);
        public abstract ITreasureItem GetTreasureItem(int index);
        public abstract IInventoryItem GetWardrobeItem(int index);
        public abstract uint GetWardrobeItemCount(ushort iD);
        public abstract uint GetWardrobeItemCountByIndex(byte index);
        public abstract int GetWardrobeItemIDByIndex(byte index);
        public abstract bool LocationHas(ushort ID, InventoryType location);
    }
}