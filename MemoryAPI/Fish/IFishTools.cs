namespace MemoryAPI
{
    public interface IFishTools
    {
        bool FishOnLine { get; }
        int HPCurrent { get; set; }
        int HPMax { get; }
        IFishID ID { get; }
        int OnLineTime { get; }
        RodAlign RodPosition { get; }
        int Timeout { get; set; }

        bool FightFish();
        bool SetFishTimeOut(short value);
        bool SetHP(int value);
    }
}