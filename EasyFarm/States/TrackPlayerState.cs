using System;
using System.Collections.Generic;
using System.Linq;
using EasyFarm.Classes;
using MemoryAPI;
using MemoryAPI.Navigation;

namespace EasyFarm.States
{
    public class TrackPlayerState : BaseState
    {
        private const int HistoryPositionLimit = 10;

        private readonly Queue<Position> _positionHistory = new Queue<Position>();

        public TrackPlayerState(IMemoryAPI fface) : base(fface)
        {
        }

        public override void RunComponent()
        {
            var position = fface.Player.Position;
            _positionHistory.Enqueue(Helpers.ToPosition(position.X, position.Y, position.Z, position.H));
            if (_positionHistory.Count == 100) _positionHistory.Dequeue();
            Player.Instance.IsMoving = IsMoving();
        }

        public bool IsMoving()
        {
            var changeInX = _positionHistory.Average(positon => positon.X) - fface.Player.PosX;
            var changeInY = _positionHistory.Average(position => position.Y) - fface.Player.PosY;
            return Math.Abs(changeInX - changeInY) <= 0;
        }
    }
}