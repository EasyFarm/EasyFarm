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
using MemoryAPI;
using MemoryAPI.Chat;
using MemoryAPI.Windower;

namespace EasyFarm.Tests.TestTypes
{
    public class FakeMemoryAPI : IMemoryAPI
    {
        public INavigatorTools Navigator { get; set; }
        public INPCTools NPC { get; set; }
        public System.Collections.Generic.Dictionary<byte, IPartyMemberTools> PartyMember { get; set; }
        public IPlayerTools Player { get; set; }
        public ITargetTools Target { get; set; }
        public ITimerTools Timer { get; set; }
        public IWindowerTools Windower { get; set; }
        public IChatTools Chat { get; set; }
    }
}
