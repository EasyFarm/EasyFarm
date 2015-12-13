namespace MemoryAPI
{
    public abstract class AbstractMenuTools : IMenuTools
    {
        public virtual short DialogID { get; }
        public virtual int DialogOptionCount { get; }
        public virtual short DialogOptionIndex { get; }
        public virtual IDialogText DialogText { get; }
        public virtual string Help { get; }
        public virtual bool IsOpen { get; }
        public virtual ThreadStatus lastTradeMenuStatus { get; }
        public virtual int MenuIndex { get; set; }
        public virtual string Name { get; }
        public virtual string Selection { get; }
        public virtual byte ShopQuantity { get; }
        public virtual byte ShopQuantityMax { get; }

        public abstract IDialogText GetDialogText();
        public abstract IDialogText GetDialogText(LineSettings lineSettings);
        public abstract void OpenTradeMenu(int TargetID);
        public abstract bool SetCraftItems(Structures.NPCTRADEINFO sTinfo);
        public abstract bool SetNPCTradeInformation(Structures.NPCTRADEINFO sTinfo);
        public abstract bool SetTradeGil(uint Gil);
    }
}