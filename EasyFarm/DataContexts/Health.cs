namespace EasyFarm.DataContexts
{
    public class Health
    {
        public long Id { get; set; }
        public bool Enabled { get; set; }
        public int High { get; set; }
        public int Low { get; set; }
    }
}
