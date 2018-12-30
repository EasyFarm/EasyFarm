using System;
using EasyFarm.Classes;
using MemoryAPI;

namespace EasyFarm.Context
{
    public class Player : IPlayer
    {
        private readonly IMemoryAPI _memoryAPI;
        private readonly UnitService _unitService;

        public Player(IMemoryAPI memoryAPI)
        {
            _memoryAPI = memoryAPI;
            _unitService = new UnitService(memoryAPI);
        }

        public Status Status
        {
            get => _memoryAPI.Player.Status;
            set => throw new NotImplementedException();
        }

        public int HppCurrent
        {
            get => _memoryAPI.Player.HPPCurrent;
            set => throw new NotImplementedException();
        }

        public bool HasAggro
        {
            get => _unitService.HasAggro;
            set => throw new NotImplementedException();
        }

        public Zone Zone
        {
            get => _memoryAPI.Player.Zone;
            set => throw new NotImplementedException();
        }

        public int Str
        {
            get => _memoryAPI.Player.Stats.Str;
            set => throw new NotImplementedException();
        }

        public int MppCurrent
        {
            get => _memoryAPI.Player.MPPCurrent;
            set => throw new NotImplementedException();
        }
    }
}
