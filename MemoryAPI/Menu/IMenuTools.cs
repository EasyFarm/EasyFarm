namespace MemoryAPI
{
    public interface IMenuTools
    {
        short DialogID { get; }
        int DialogOptionCount { get; }
        short DialogOptionIndex { get; }
        IDialogText DialogText { get; }
        string Help { get; }
        bool IsOpen { get; }
        ThreadStatus lastTradeMenuStatus { get; }
        int MenuIndex { get; set; }
        string Name { get; }
        string Selection { get; }
        byte ShopQuantity { get; }
        byte ShopQuantityMax { get; }

        IDialogText GetDialogText();
        IDialogText GetDialogText(LineSettings lineSettings);
        void OpenTradeMenu(int TargetID);
        bool SetCraftItems(Structures.NPCTRADEINFO sTinfo);
        bool SetNPCTradeInformation(Structures.NPCTRADEINFO sTinfo);
        bool SetTradeGil(uint Gil);
    }
}