
/*///////////////////////////////////////////////////////////////////
<EasyFarm, general farming utility for FFXI.>
Copyright (C) <2013 - 2014>  <Zerolimits>

This program is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

This program is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
*////////////////////////////////////////////////////////////////////

﻿// Author: Myrmidon
// Site: FFEVO.net
// All credit to him!
// http://www.ffevo.net/topic/2726-autodetecting-characters-logged-in-out-for-multiboxers/

using System.Diagnostics;
using FFACETools;

namespace EasyFarm.ProcessTools
{
    public class FFInstance
    {
        public Process MyProcess { get; private set; }
        public FFACE Instance { get; private set; }

        #region Constructor
        public FFInstance(Process proc)
        {
            MyProcess = proc;
            Instance = new FFACE(MyProcess.Id);
        }
        #endregion

        #region ToString
        public override string ToString()
        {
            if (!Valid) { return "DELETED"; }
            else { return MyProcess.MainWindowTitle; }
        }
        #endregion

        #region Valid Check
        public bool Valid
        {
            get
            {
                return (MyProcess == null ? false : !MyProcess.HasExited);
            }
        }
        #endregion

        #region Equals Override
        public override bool Equals(object obj)
        {
            FFInstance temp = obj as FFInstance;
            if (temp == null) { return false; }
            return MyProcess.Id == temp.MyProcess.Id;
        }

        public override int GetHashCode() { return base.GetHashCode(); }
        #endregion
    }
}