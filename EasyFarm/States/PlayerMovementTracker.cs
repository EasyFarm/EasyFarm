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

using System;
using System.Collections.Generic;
using System.Linq;
using EasyFarm.Classes;
using MemoryAPI;
using MemoryAPI.Navigation;

namespace EasyFarm.States
{
    public class PlayerMovementTracker
    {
        private readonly IMemoryAPI _fface;
        private readonly Queue<Position> _positionHistory = new Queue<Position>();

        public PlayerMovementTracker(IMemoryAPI fface)
        {
            _fface = fface;
        }

        public void RunComponent()
        {
            var position = _fface.Player.Position;
            _positionHistory.Enqueue(Helpers.ToPosition(position.X, position.Y, position.Z, position.H));
            if (_positionHistory.Count >= 15) _positionHistory.Dequeue();
            Player.Instance.IsMoving = IsMoving();
        }

        public bool IsMoving()
        {
            var changeInX = _positionHistory.Average(positon => positon.X) - _fface.Player.PosX;
            var changeInZ = _positionHistory.Average(position => position.Z) - _fface.Player.PosZ;
            return Math.Abs(changeInX) + Math.Abs(changeInZ) > 0;
        }
    }
}