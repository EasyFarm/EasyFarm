﻿// ///////////////////////////////////////////////////////////////////
// This file is a part of EasyFarm for Final Fantasy XI
// Copyright (C) 2013 Mykezero
//  
// EasyFarm is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//  
// EasyFarm is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// If not, see <http://www.gnu.org/licenses/>.
// ///////////////////////////////////////////////////////////////////
using System;

namespace MemoryAPI.Navigation
{
    public class Position
    {
        public float H { get; set; }

        public float X { get; set; }

        public float Y { get; set; }

        public float Z { get; set; }

        public override string ToString()
        {
            return "(X: " + X + " Z: " + Z + ")";
        }

        public override int GetHashCode()
        {
            return (X.GetHashCode() ^ Y.GetHashCode() ^ Z.GetHashCode() ^ H.GetHashCode());
        }

        public override bool Equals(object obj)
        {
            var other = obj as Position;
            if (other == null) return false;

            var deviation = Math.Abs(this.X - other.X) + 
                Math.Abs(this.Y - other.Y) + 
                Math.Abs(this.Z - other.Z) + 
                Math.Abs(this.H - other.H);

            return Math.Abs(deviation) <= 0;
        }

        public double Distance(Position other)
        {
            return Math.Sqrt(Math.Pow(X - other.X, 2) + Math.Pow(Z - other.Z, 2));
        }

        public float[] ToDetourPosition()
        {
            float[] detourPosition = new float[3];

            detourPosition[0] = X;
            detourPosition[1] = -Y;
            detourPosition[2] = -Z;

            return detourPosition;
        }
    }
}