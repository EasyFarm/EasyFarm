namespace MemoryAPI
{
    public abstract class AbstractVanaTime : IVanaTime
    {
        public virtual byte Day { get; set; }
        public virtual Weekday DayType { get; set; }
        public virtual byte Hour { get; set; }
        public virtual byte Minute { get; set; }
        public virtual byte Month { get; set; }
        public virtual byte MoonPercent { get; set; }
        public virtual MoonPhase MoonPhase { get; set; }
        public virtual byte Second { get; set; }
        public virtual bool Waxing { get; set; }
        public virtual short Year { get; set; }

        public abstract string GetDayOfWeekName(Weekday day);
        public abstract string GetMoonPhaseName(MoonPhase phase);
    }
}