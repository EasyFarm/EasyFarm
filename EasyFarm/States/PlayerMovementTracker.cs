// ///////////////////////////////////////////////////////////////////
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
using System.Collections.Generic;
using System.Linq;
using EasyFarm.Classes;
using EliteMMO.API;
using MemoryAPI;
using MemoryAPI.Navigation;

namespace EasyFarm.States
{
    public class PlayerMovementTracker
    {
        private static readonly object LockObject = new object();

        private readonly IMemoryAPI _fface;
        struct Tuple
        {
            public Position position;
            public DateTime time;
        }

        private readonly Queue<Tuple> _positionHistory = new Queue<Tuple>();

        public PlayerMovementTracker(IMemoryAPI fface)
        {
            _fface = fface;
        }

        public void RunComponent()
        {
            lock (LockObject)
            {
                var position = _fface.Player.Position;
                Tuple tuple = new Tuple() { position = position, time = DateTime.Now };
                _positionHistory.Enqueue(tuple);
                if (_positionHistory.Count >= 30) _positionHistory.Dequeue();
            }
            var isMoving = IsMoving();
            var isStuck = IsStuck();
            Player.Instance.IsMoving = isMoving;
            Player.Instance.IsStuck = isMoving && isStuck;

            _fface.Navigator.IsStuck = isStuck;
        }

        public bool IsMoving()
        {
            lock (LockObject)
            {
                var changeInX = _positionHistory.Average(tuple => tuple.position.X) - _fface.Player.PosX;
                var changeInZ = _positionHistory.Average(tuple => tuple.position.Z) - _fface.Player.PosZ;
                
                return Math.Abs(changeInX) + Math.Abs(changeInZ) > 0;
            }
        }

        public bool IsStuck()
        {
            lock (LockObject)
            {
                var changeInX = _positionHistory.Average(tuple => tuple.position.X) - _fface.Player.PosX;
                var changeInZ = _positionHistory.Average(tuple => tuple.position.Z) - _fface.Player.PosZ;

                var checkTime = DateTime.Now - _positionHistory.Last().time;
                var expectedDelta = 0.3; //(checkTime.TotalSeconds / _api.Player.Speed) * 0.25;
                var dchange = Math.Pow(changeInX, 2) + Math.Pow(changeInZ, 2);
                var isStuck = Math.Abs(dchange) < expectedDelta;

                return isStuck;
            }
        }
    }
}