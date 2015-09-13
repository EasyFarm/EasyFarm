using AutoMapper;
using EasyFarm.Classes;
using FFACETools;

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
            return Mapper.Map<FFACE.Position, Position>(_fface.Player.Position);
        }
    }
}
