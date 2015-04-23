using System.Collections.Generic;
using System.Linq;
using FFACETools;

namespace EasyFarm.Classes
{
    /// <summary>
    ///     A generic bounding box that calculates its values
    ///     from an list of positions.
    ///     ///
    /// </summary>
    public class BoundingBox
    {
        /// <summary>
        ///     Generate the bounding box from the positional data.
        /// </summary>
        /// <param name="positions"></param>
        public BoundingBox(IEnumerable<FFACE.Position> positions)
        {
            // Generate X Min and Max between all positions. 
            XMin = positions.Min(position => position.X);
            XMax = positions.Max(position => position.X);

            // Generate Y Min and Max between all positions. 
            ZMin = positions.Min(position => position.Z);
            ZMax = positions.Max(position => position.Z);
        }

        /// <summary>
        ///     The lowest x value.
        /// </summary>
        public float XMin { get; set; }

        /// <summary>
        ///     The highest x value.
        /// </summary>
        public float XMax { get; set; }

        /// <summary>
        ///     The lowest y value.
        /// </summary>
        public float ZMin { get; set; }

        /// <summary>
        ///     The highest y value.
        /// </summary>
        public float ZMax { get; set; }
    }
}