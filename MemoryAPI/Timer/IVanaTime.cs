namespace MemoryAPI
{
    public interface IVanaTime
    {
        byte Day { get; set; }
        Weekday DayType { get; set; }
        byte Hour { get; set; }
        byte Minute { get; set; }
        byte Month { get; set; }
        byte MoonPercent { get; set; }
        MoonPhase MoonPhase { get; set; }
        byte Second { get; set; }
        bool Waxing { get; set; }
        short Year { get; set; }

        string GetDayOfWeekName(Weekday day);
        string GetMoonPhaseName(MoonPhase phase);
        string ToString();
    }
}