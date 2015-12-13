using AutoMapper;
using EasyFarm.Classes;
using MemoryAPI;

namespace EasyFarm.Memory
{
    public class MemorySource : IMemorySource
    {
        private FFACE _fface;

        public MemorySource(FFACE fface)
        {
            this._fface = fface;
        }

        public virtual Position GetPlayerPosition()
        {
            return Mapper.Map<IPosition, Position>(_fface.Player.Position);
        }
    }
}
