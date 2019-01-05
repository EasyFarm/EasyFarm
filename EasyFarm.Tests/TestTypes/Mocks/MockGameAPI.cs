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
using System.Collections.Generic;
using MemoryAPI;
using MemoryAPI.Chat;
using MemoryAPI.Resources;
using MemoryAPI.Windower;

namespace EasyFarm.Tests.TestTypes.Mocks
{
    public class MockGameAPI : IMemoryAPI
    {
        private readonly IMemoryAPI _instance;

        public MockEliteAPI Mock { get; }

        public MockGameAPI()
        {
            Mock = new MockEliteAPI();
            _instance = new MockEliteAPIAdapter(Mock);
        }

        public INavigatorTools Navigator
        {
            get { return _instance.Navigator; }
            set { _instance.Navigator = value; }
        }

        public INPCTools NPC
        {
            get { return _instance.NPC; }
            set { _instance.NPC = value; }
        }

        public Dictionary<byte, IPartyMemberTools> PartyMember
        {
            get { return _instance.PartyMember; }
            set { _instance.PartyMember = value; }
        }

        public IPlayerTools Player
        {
            get { return _instance.Player; }
            set { _instance.Player = value; }
        }

        public ITargetTools Target
        {
            get { return _instance.Target; }
            set { _instance.Target = value; }
        }

        public ITimerTools Timer
        {
            get { return _instance.Timer; }
            set { _instance.Timer = value; }
        }

        public IWindowerTools Windower
        {
            get { return _instance.Windower; }
            set { _instance.Windower = value; }
        }

        public IChatTools Chat
        {
            get { return _instance.Chat; }
            set { _instance.Chat = value; }
        }

        public IResourcesTools Resource
        {
            get { return _instance.Resource; }
            set { _instance.Resource = value; }
        }
    }
}