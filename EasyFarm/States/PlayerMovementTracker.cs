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
            return (Math.Abs(changeInX) + Math.Abs(changeInZ)) > 0;
        }
    }
}