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
using MemoryAPI.Navigation;

namespace MemoryAPI
{
    public interface INPCTools
    {
        int ClaimedID(int id);
        double Distance(int id);
        Position GetPosition(int id);
        short HPPCurrent(int id);
        bool IsActive(int id);
        bool IsClaimed(int id);
        bool IsRendered(int id);
        string Name(int id);
        NpcType NPCType(int id);
        float PosX(int id);
        float PosY(int id);
        float PosZ(int id);
        Status Status(int id);
        int PetID(int id);
    }
}