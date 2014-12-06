
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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EasyFarm.GameData
{
    public class Waypoint
    {
        public Waypoint() 
        {
            _position = new FFACE.Position();
        }

        private FFACE.Position _position;

        public Waypoint(FFACE.Position position)
        {
            _position = position;
        }

        public float X
        {
            get { return _position.X; }
            set { _position.X = value; }
        }

        public float Y
        {
            get { return _position.Y; }
            set { _position.Y = value; }
        }

        public float Z
        {
            get { return _position.Z; }
            set { _position.Z = value; }
        }

        public float H
        {
            get { return _position.H; }
            set { _position.H = value; }
        }

        public FFACE.Position Position
        {
            get { return _position; }
            set { _position = value; }
        }

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
            return this.Position == (obj as Waypoint).Position;
        }
    }
}
