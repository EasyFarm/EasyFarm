// ///////////////////////////////////////////////////////////////////
// This file is a part of EasyFarm for Final Fantasy XI
// Copyright (C) 2013-2017 Mykezero
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
using System.Collections.Generic;
using MemoryAPI;
using MemoryAPI.Navigation;

namespace EasyFarm.Tests.TestTypes
{
    public class FakePlayer : IPlayerTools
    {
        private static readonly IEnumerator<float> CastPercentExEnumerator = 
            CastPercentExValues()
            .GetEnumerator();

        public float CastPercentEx
        {
            get
            {
                var value = CastPercentExEnumerator.Current;
                CastPercentExEnumerator.MoveNext();
                return value;
            }
        }
        public int HPPCurrent { get; set; }
        public int ID { get; set; }
        public int MPCurrent { get; set; }
        public int MPPCurrent { get; set; }
        public string Name { get; set; }
        public Position Position { get; set; }
        public float PosX { get; set; }
        public float PosY { get; set; }
        public float PosZ { get; set; }
        public Structures.PlayerStats Stats { get; set; }
        public Status Status { get; set; }
        public StatusEffect[] StatusEffects { get; set; }
        public int TPCurrent { get; set; }
        public Zone Zone { get; set; }
        public Job Job { get; set; }
        public Job SubJob { get; set; }


        public static IEnumerable<float> CastPercentExValues()
        {
            yield return 0;
            yield return 0;
            yield return 0;

            yield return 50;
            yield return 50;
            yield return 50;

            while (true)
            {
                yield return 100;
            }           
        }
    }
}
