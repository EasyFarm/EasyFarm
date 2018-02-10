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

using System;

namespace EasyFarm.States
{
    public abstract class BaseState : IState, IComparable
    {
        public virtual bool Enabled { get; set; }

        public virtual int Priority { get; set; }

        public virtual bool Check()
        {
            return false;
        }

        public virtual void Enter()
        {
        }

        public virtual void Exit()
        {
        }

        public virtual void Run()
        {
        }

        public virtual int CompareTo(object obj)
        {
            if (!(obj is IState other)) return 1;
            return -Priority.CompareTo(other.Priority);
        }
    }
}