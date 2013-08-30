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
    ///   A decorator that allows you to execute code only if some condition is met. It does not 'break' the current
    ///   tree if the condition fails, or children fail.
    /// 		 
    ///   This is useful for "if I need to, go ahead, otherwise, ignore" in sequences.
    /// 		 
    ///   It can be thought of as an optional execution.
    /// </summary>
    /// <remarks>
    ///   Created 1/13/2011.
    /// </remarks>
    public class DecoratorContinue : Decorator
    {
        public DecoratorContinue(CanRunDecoratorDelegate func, Composite decorated)
            : base(func, decorated)
        {
        }

        public DecoratorContinue(Composite child)
            : base(child)
        {
        }

        private RunStatus GetContinuationStatus()
        {
            // Selectors run until we fail.
            if (Parent is Selector)
            {
                return RunStatus.Failure;
            }
            // Everything else, we want to 'succeed'.
            return RunStatus.Success;
        }

        public override IEnumerable<RunStatus> Execute(object context)
        {
            if (!CanRun(context))
            {
                yield return RunStatus.Success;
                yield break;
            }

            DecoratedChild.Start(context);
            while (DecoratedChild.Tick(context) == RunStatus.Running)
            {
                yield return RunStatus.Running;
            }

            DecoratedChild.Stop(context);
            if (DecoratedChild.LastStatus == RunStatus.Failure)
            {
                yield return GetContinuationStatus();
                yield break;
            }

            // Note: if the condition was met, and we succeeded in running the children, we HAVE to tell our parent
            // that we've ran successfully, or we'll skip down to the next child, regardless of whether we ran, or not.
            yield return RunStatus.Success;
            yield break;
        }
    }
}