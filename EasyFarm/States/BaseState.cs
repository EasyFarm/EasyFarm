/*///////////////////////////////////////////////////////////////////
<EasyFarm, general farming utility for FFXI.>
Copyright (C) <2013>  <Zerolimits>

This program is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

This program is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
*/
///////////////////////////////////////////////////////////////////

using System;
using MemoryAPI;

namespace EasyFarm.States
{
    public abstract class BaseState : IState, IComparable
    {
        /// <summary>
        ///     The game session.
        /// </summary>
        protected IMemoryAPI fface { get; set; }

        protected BaseState(IMemoryAPI fface)
        {
            this.fface = fface;
        }

        public virtual bool Enabled { get; set; }

        public virtual int Priority { get; set; }

        public virtual bool Check() { return false; }

        public virtual void Enter() { }

        public virtual void Exit() { }

        public virtual void Run() { }

        public virtual int CompareTo(object obj)
        {
            var other = obj as IState;
            if (other == null) return 1;
            return -Priority.CompareTo(other.Priority);
        }
    }
}