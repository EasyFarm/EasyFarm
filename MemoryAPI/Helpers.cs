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
using EliteMMO.API;
using MemoryAPI.Navigation;

namespace MemoryAPI
{
    public class Helpers
    {
        public static Position ToPosition(float x, float y, float z, float h)
        {
            var position = new Position
            {
                X = x,
                Y = y,
                Z = z,
                H = h
            };

            return position;
        }

        public static Status ToStatus(EntityStatus status)
        {
            switch (status)
            {
                case EntityStatus.Idle:
                    return Status.Standing;
                case EntityStatus.Engaged:
                    return Status.Fighting;
                case EntityStatus.Dead:
                    return Status.Dead1;
                case EntityStatus.DeadEngaged:
                    return Status.Dead2;
                case EntityStatus.Event:
                    return Status.Event;
                case EntityStatus.Healing:
                    return Status.Healing;
                default:
                    return Status.Unknown;
            }
        }

        public static NpcType GetNpcType(EliteAPI.EntityEntry entity)
        {
            if (entity.WarpPointer == 0) return NpcType.InanimateObject;
            if (IsOfType(entity.SpawnFlags, (int)NpcType.Mob)) return NpcType.Mob;
            if (IsOfType(entity.SpawnFlags, (int)NpcType.NPC)) return NpcType.NPC;
            if (IsOfType(entity.SpawnFlags, (int)NpcType.PC)) return NpcType.PC;
            if (IsOfType(entity.SpawnFlags, (int)NpcType.Self)) return NpcType.Self;
            return NpcType.InanimateObject;
        }

        private static bool IsOfType(int one, int other)
        {
            return (one & other) == other;
        }
    }
}
