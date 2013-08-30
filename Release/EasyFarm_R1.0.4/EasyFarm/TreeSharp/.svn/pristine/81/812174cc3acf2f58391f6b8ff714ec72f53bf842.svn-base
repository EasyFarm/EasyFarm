#region License

// A simplistic Behavior Tree implementation in C#
// Copyright (C) 2010-2011 ApocDev apocdev@gmail.com
// 
// This file is part of TreeSharp
// 
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU Lesser General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <http://www.gnu.org/licenses/>.

#endregion

using System.Collections.Generic;

namespace TreeSharp
{
    public delegate RunStatus ActionDelegate(object context);

    public delegate void ActionSucceedDelegate(object context);

    /// <summary>
    ///   The base Action class. A simple, easy to use, way to execute actions, and return their status of execution.
    ///   These are normally considered 'atoms' in that they are executed in their entirety.
    /// </summary>
    public class Action : Composite
    {
        public Action()
        {
        }

        public Action(ActionDelegate action = null)
        {
            Runner = action;
        }

        public Action(ActionSucceedDelegate action = null)
        {
            SucceedRunner = action;
        }

        public ActionDelegate Runner { get; private set; }

        public ActionSucceedDelegate SucceedRunner { get; private set; }

        /// <summary>
        ///   Runs this action, and returns a <see cref = "RunStatus" /> describing it's current state of execution.
        ///   If this method is not overriden, it returns <see cref = "RunStatus.Failure" />.
        /// </summary>
        /// <returns></returns>
        protected virtual RunStatus Run(object context)
        {
            return RunStatus.Failure;
        }

        public override sealed IEnumerable<RunStatus> Execute(object context)
        {
            if (Runner != null)
            {
                yield return Runner(context);
            }
            else if (SucceedRunner != null)
            {
                SucceedRunner(context);
                yield return RunStatus.Success;
            }
            else
            {
                yield return Run(context);
            }
        }
    }
}