using EasyFarm.Classes;
using MemoryAPI;

namespace EasyFarm
{
    public class Helpers
    {
        public static IPosition CreatePosition(float x, float y, float z, float h)
        {
            var position = new Position();

            position.X = x;
            position.Y = y;
            position.Z = z;
            position.H = h;

            return position;
        }
    }
}
