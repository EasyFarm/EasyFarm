namespace EasyFarm.Classes
{
  public class Range
  {
      public int Low { get; }
      public int High { get; }

      public Range(int low, int high)
      {
          Low = low;
          High = high;
      }

      public bool InRange(int value)
      {
          return Low <= value && value <= High;
      }

      public bool NotSet()
      {
          return Low == 0 && High == 0;
      }
  }
}
