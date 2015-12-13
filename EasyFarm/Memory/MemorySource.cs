using AutoMapper;
using EasyFarm.Classes;
using MemoryAPI;

namespace EasyFarm.Memory
{
    public class MemorySource : IMemorySource
    {
        private MemoryWrapper _fface;

        public MemorySource(MemoryWrapper fface)
        {
            this._fface = fface;
        }

        public virtual Position GetPlayerPosition()
        {
            return Mapper.Map<IPosition, Position>(_fface.Player.Position);
        }
    }
}
