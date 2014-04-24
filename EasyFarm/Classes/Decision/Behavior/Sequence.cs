
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
*/
///////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EasyFarm.Classes
{
    public abstract class Sequence : Behavior
    {
        public List<Behavior> _behaviors;

        public Sequence() { _behaviors = new List<Behavior>(); }

        public override TerminationStatus Execute()
        {
            foreach (var child in _behaviors)
            {
                if (child.CanExecute())
                {
                    var status = child.Execute();
                    if (!status.Equals(TerminationStatus.Success))
                    {
                        return status;
                    }
                }
            }

            return TerminationStatus.Success;
        }

        public override bool CanExecute() { return true; }
    }
}
