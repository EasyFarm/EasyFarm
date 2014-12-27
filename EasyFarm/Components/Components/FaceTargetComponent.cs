
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

using EasyFarm.ViewModels;
using FFACETools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZeroLimits.XITool.Classes;

namespace EasyFarm.Components
{
    /// <summary>
    /// Faces the enemy every second. 
    /// </summary>
    public class FaceTargetComponent : MachineComponent
    {
        public FFACE FFACE { get; set; }

        public Unit Target
        {
            get { return AttackContainer.TargetUnit; }
            set { AttackContainer.TargetUnit = value; }
        }

        public FaceTargetComponent(FFACE fface)
        { 
            this.FFACE = fface;
        }

        public override bool CheckComponent()
        {
            return Target != null && !Target.IsDead;
        }

        public override void EnterComponent() { }

        public override void RunComponent() 
        {
            FFACE.Navigator.FaceHeading(Target.Position);
        }

        public override void ExitComponent() { }
    }
}
