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

using MemoryAPI;
using System;
using System.Linq;
using MemoryAPI.Navigation;

namespace EasyFarm.Classes
{
    public class Unit : IUnit
    {
        public Unit(IMemoryAPI fface, int id)
        {
            // Set this unit's session data. 
            _fface = fface;

            // Set the internal id. 
            Id = id;

            // Set the NPC information.
            _npc = _fface.NPC;
        }

        /// <summary>
        ///     Holds all the game's data.
        /// </summary>
        private readonly IMemoryAPI _fface;

        /// <summary>
        ///     Holds the data about units.
        /// </summary>
        private readonly INPCTools _npc;

        /// <summary>
        ///     The unit's id.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        ///     The unit's claim id; zero for unclaimed.
        /// </summary>
        public int ClaimedId
        {
            get { return _npc.ClaimedID(Id); }
        }

        /// <summary>
        ///     The unit's distace from the player.
        /// </summary>
        public double Distance
        {
            get { return _npc.Distance(Id); }
        }

        /// <summary>
        ///     The unit's position.
        /// </summary>
        public Position Position
        {
            get
            {
                var position = _npc.GetPosition(Id);

                return Helpers.ToPosition(
                    position.X, 
                    position.Y, position.Z, 
                    position.H);
            }
        }

        /// <summary>
        ///     The unit's health as a percent.
        /// </summary>
        public short HppCurrent
        {
            get { return _npc.HPPCurrent(Id); }
        }

        /// <summary>
        ///     Whether this unit is active.
        /// </summary>
        public bool IsActive
        {
            get { return _npc.IsActive(Id); }
        }

        /// <summary>
        ///     Whether this unit is claimed by some player.
        /// </summary>
        public bool IsClaimed
        {
            get { return _npc.IsClaimed(Id); }
        }

        /// <summary>
        ///     Whether this unit is visible to the player.
        /// </summary>
        public bool IsRendered
        {
            get { return _npc.IsRendered(Id); }
        }

        /// <summary>
        ///     The unit's name.
        /// </summary>
        public string Name
        {
            get { return _npc.Name(Id); }
        }

        /// <summary>
        ///     The unit's npc type
        /// </summary>
        public NpcType NpcType
        {
            get { return _npc.NPCType(Id); }
        }

        /// <summary>
        ///     The unit's x coordinate.
        /// </summary>
        public float PosX
        {
            get { return _npc.PosX(Id); }
        }

        /// <summary>
        ///     The unit's y coordinate.
        /// </summary>
        public float PosY
        {
            get { return _npc.PosY(Id); }
        }

        /// <summary>
        ///     The unit's z coordinate.
        /// </summary>
        public float PosZ
        {
            get { return _npc.PosZ(Id); }
        }

        /// <summary>
        ///     The unit's status.
        /// </summary>
        public Status Status
        {
            get { return _npc.Status(Id); }
        }

        public bool MyClaim
        {
            // Using fface.PartyMember[0].ServerID until fface.Player.PlayerServerID is fixed. 
            get { return ClaimedId == _fface.PartyMember[0].ServerID; }
        }

        /// <summary>
        ///     If the unit has aggroed our player.
        /// </summary>
        public bool HasAggroed
        {
            get { return (!IsClaimed || MyClaim) && Status == Status.Fighting; }
        }

        /// <summary>
        ///     If the unit is dead.
        /// </summary>
        public bool IsDead
        {
            get { return Status == Status.Dead1 || Status == Status.Dead2 || HppCurrent <= 0; }
        }

        /// <summary>
        ///     If a party or alliance member has claim on the unit.
        /// </summary>
        public bool PartyClaim
        {
            get
            {
                for (byte i = 0; i < _fface.PartyMember.Count; i++)
                {
                    if (_fface.PartyMember[i].ServerID != 0 && ClaimedId == _fface.PartyMember[i].ServerID)
                    {
                        return true;
                    }
                }
                return false;
            }
        }

        /// <summary>
        ///     The vertical distance between this unit and our player.
        /// </summary>
        public double YDifference
        {
            get { return Math.Abs(PosY - _fface.Player.PosY); }
        }

        public bool IsPet
        {
            get
            {
                var playerIds = Enumerable.Range(0, 2048)
                    .Where(x => _npc.NPCType(x) == NpcType.PC)
                    .ToList();

                return playerIds.Any(x => _npc.PetID(x) == Id);
            }
        }
    }
}