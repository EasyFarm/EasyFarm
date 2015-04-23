/*///////////////////////////////////////////////////////////////////
<EasyFarm, general farming utility for FFXI.>
Copyright (C) <2013>  <Zerolimits>

This program is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

This program is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
*/
///////////////////////////////////////////////////////////////////

using FFACETools;

namespace EasyFarm.Classes
{
    public class Waypoint
    {
        public Waypoint()
        {
            Position = new FFACE.Position();
        }

        public Waypoint(FFACE.Position position)
        {
            Position = position;
        }

        public float X
        {
            get { return Position.X; }
            set { Position.X = value; }
        }

        public float Y
        {
            get { return Position.Y; }
            set { Position.Y = value; }
        }

        public float Z
        {
            get { return Position.Z; }
            set { Position.Z = value; }
        }

        public float H
        {
            get { return Position.H; }
            set { Position.H = value; }
        }

        public FFACE.Position Position { get; set; }

        public override string ToString()
        {
            return "X: " + Position.X + "Z: " + Position.Z;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            return Position == (obj as Waypoint).Position;
        }
    }
}