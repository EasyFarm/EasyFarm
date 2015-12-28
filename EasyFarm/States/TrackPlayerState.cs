using System;
using System.Linq;
using EasyFarm.Classes;
using EasyFarm.Collections;
using MemoryAPI;
using MemoryAPI.Navigation;

namespace EasyFarm.States
{
    public class TrackPlayerState : BaseState
    {
        private const int HistoryPositionLimit = 10;

        private readonly ThresholdQueue<Position> _positionHistory =
            new ThresholdQueue<Position>(HistoryPositionLimit, .75);

        public TrackPlayerState(IMemoryAPI fface) : base(fface)
        {
        }

        public override void RunComponent()
        {
            var position = fface.Player.Position;
            _positionHistory.AddItem(Helpers.ToPosition(position.X, position.Y, position.Z, position.H));
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