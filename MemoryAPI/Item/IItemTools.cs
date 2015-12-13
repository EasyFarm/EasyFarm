namespace MemoryAPI
{
    public interface IItemTools
    {
        short CaseCount { get; }
        short CaseMax { get; }
        uint CurrentGil { get; }
        short InventoryCount { get; }
        short InventoryMax { get; }
        short LockerCount { get; }
        short LockerMax { get; }
        short SackCount { get; }
        short SackMax { get; }
        short SafeCount { get; }
        short SafeMax { get; }
        short SatchelCount { get; }
        short SatchelMax { get; }
        int SelectedItemID { get; }
        short SelectedItemIndex { get; }
        string SelectedItemName { get; }
        int SelectedItemNum { get; }
        short StorageCount { get; }
        short StorageMax { get; }
        short TemporaryCount { get; }
        short TemporaryMax { get; }
        short WardrobeCount { get; }
        short WardrobeMax { get; }

        IInventoryItem GetCaseItem(int index);
        uint GetCaseItemCount(ushort iD);
        uint GetCaseItemCountByIndex(byte index);
        int GetCaseItemIDByIndex(byte index);

        IInventoryItem GetEquippedItem(EquipSlot slot);
        uint GetEquippedItemCount(EquipSlot slot);
        int GetEquippedItemID(EquipSlot slot);
        byte GetEquippedItemIndex(EquipSlot slot);
        byte GetEquippedItemLocation(EquipSlot slot);
        int GetFirstIndexByItemID(int ID, InventoryType location);

        IInventoryItem GetInventoryItem(int index);
        uint GetInventoryItemCount(ushort iD);
        uint GetInventoryItemCountByIndex(int index);
        int GetInventoryItemIDByIndex(byte index);

        IInventoryItem GetItem(int index, InventoryType location);
        uint GetItemCount(int iD, InventoryType location);
        uint GetItemCountByIndex(int index, InventoryType location);
        int GetItemIDByIndex(int index, InventoryType location);
        System.Collections.Generic.List<IInventoryItem> GetItemList(int iD, InventoryType location);

        IInventoryItem GetLockerItem(int index);
        uint GetLockerItemCount(ushort iD);
        uint GetLockerItemCountByIndex(byte index);
        int GetLockerItemIDByIndex(byte index);

        IInventoryItem GetSackItem(int index);
        uint GetSackItemCount(ushort iD);
        uint GetSackItemCountByIndex(byte index);
        int GetSackItemIDByIndex(byte index);

        IInventoryItem GetSafeItem(int index);
        uint GetSafeItemCount(ushort iD);
        uint GetSafeItemCountByIndex(byte index);
        int GetSafeItemIDByIndex(byte index);

        IInventoryItem GetSatchelItem(int index);
        uint GetSatchelItemCount(ushort iD);
        uint GetSatchelItemCountByIndex(byte index);
        int GetSatchelItemIDByIndex(byte index);

        IInventoryItem GetStorageItem(int index);
        uint GetStorageItemCount(ushort iD);
        uint GetStorageItemCountByIndex(byte index);
        int GetStorageItemIDByIndex(byte index);

        IInventoryItem GetTempItem(int index);
        uint GetTempItemCount(ushort iD);
        uint GetTempItemCountByIndex(byte index);
        int GetTempItemIDByIndex(byte index);

        ITreasureItem GetTreasureItem(int index);

        IInventoryItem GetWardrobeItem(int index);
        uint GetWardrobeItemCount(ushort iD);
        uint GetWardrobeItemCountByIndex(byte index);
        int GetWardrobeItemIDByIndex(byte index);
        bool LocationHas(ushort ID, InventoryType location);
    }
}