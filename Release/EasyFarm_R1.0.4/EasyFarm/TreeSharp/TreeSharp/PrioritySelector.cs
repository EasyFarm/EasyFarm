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
    /// <summary>
    ///   Will execute each branch of logic in order, until one succeeds. This composite
    ///   will fail only if all branches fail as well.
    /// </summary>
    public class PrioritySelector : Selector
    {
        public PrioritySelector(params Composite[] children)
            : base(children)
        {
        }

        public PrioritySelector(ContextChangeHandler contextChange, params Composite[] children)
            : this(children)
        {
            ContextChanger = contextChange;
        }

        public override IEnumerable<RunStatus> Execute(object context)
        {
            lock (Locker)
            {
                // Keep in mind; we ARE an enumerator here. So we do execute each child in tandem.
                foreach (Composite node in Children)
                {
                    // All behaviors are 'Decorators' by default. This just makes it simple.
                    // and allows us to not have another class that is nothing but a Decorator : Behavior
                    // Though; it may be a good idea in the future to add.
                    // Keep in mind!!!
                    // Start is called EVERY time we check a behavior! REGARDLESS OF IT'S RETURN VALUE!
                    // This makes sure we don't end up with a corrupted state that always returns 'Running' 
                    // when it's actualled 'Success' or 'Failed'
                    node.Start(context);
                    // If the current node is still running; return so. Don't 'break' the enumerator
                    while (node.Tick(context) == RunStatus.Running)
                    {
                        Selection = node;
                        yield return RunStatus.Running;
                    }

                    // Clear the selection... since we don't have one! Duh.
                    Selection = null;
                    // Call Stop to allow the node to cleanup anything. Since we don't need it anymore.
                    node.Stop(context);
                    // If it succeeded (since we're a selector) we return that this GroupNode
                    // succeeded in executing.
                    if (node.LastStatus == RunStatus.Success)
                    {
                        yield return RunStatus.Success;
                        yield break;
                    }

                    // XXX - Removed. This would make us use an extra 'tick' just to get to the next child composite.
                    // Still running, so continue on!
                    //yield return RunStatus.Running;
                }
                // We ran out of children, and none succeeded. Return failed.
                yield return RunStatus.Failure;
                // Make sure we tell our parent composite, that we're finished.
                yield break;
            }
        }
    }
}