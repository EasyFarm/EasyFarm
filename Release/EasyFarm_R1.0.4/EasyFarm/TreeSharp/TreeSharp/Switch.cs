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

using System;
using System.Collections.Generic;
using System.Linq;

namespace TreeSharp
{
    /// <summary>
    ///   This composite will perform a 'switch' statement to execute a specific branch of logic.
    ///   This is useful for selecting specific branches, for different types of agents. (e.g. rogue, mage, and warrior branches)
    /// </summary>
    /// <typeparam name = "T"></typeparam>
    public class Switch<T> : GroupComposite
    {
        public Switch(Func<T> statement, params SwitchArgument<T>[] args) : base(args.Select(a => a.Branch).ToArray())
        {
            Statement = statement;
            Arguments = args;
        }

        public Switch(Func<T> statement, Composite defaultArgument, params SwitchArgument<T>[] args) : this(statement, args)
        {
            Default = defaultArgument;
        }

        /// <summary>
        ///   The statement assigned to this Switch that will determine which logical branch to take.
        /// </summary>
        protected Func<T> Statement { get; set; }

        /// <summary>
        ///   The switch arguments.
        /// </summary>
        protected SwitchArgument<T>[] Arguments { get; set; }

        /// <summary>
        ///   The 'default' argument to be carried out if no other switch conditions are met.
        /// </summary>
        protected Composite Default { get; set; }

        protected void RunSwitch()
        {
            if (Arguments == null && Default == null)
            {
                throw new NullReferenceException("Switch statement has no arguments, and no default statement. Can not run.");
            }

            if (Statement == null)
            {
                throw new NullReferenceException("Switch statement is null.");
            }

            // Run the statement, and get the value for our switch.
            T value = Statement();

            // Since we can't do an *actual* switch statement,
            // this is the best we can do. It works in the same way,
            // except that it's slower, and may cause severe performance
            // hits if there are a large number of switch cases.
            if (Arguments != null)
            {
                // Make sure we don't do this query twice.
                SwitchArgument<T> arg = Arguments.First(a => a.RequiredValue.Equals(value));
                if (arg != null)
                {
                    Selection = arg.Branch;

                    // BUGFIX (http://code.google.com/p/treesharp/issues/detail?id=3)
                    return;
                }
            }

            if (Default != null)
            {
                Selection = Default;
            }
        }

        public override IEnumerable<RunStatus> Execute(object context)
        {
            RunSwitch();

            if (Selection == null)
            {
                throw new IndexOutOfRangeException("No valid statement was found to match the switch value.");
            }

            Selection.Start(context);
            while (Selection.Tick(context) == RunStatus.Running)
            {
                yield return RunStatus.Running;
            }

            Selection.Stop(context);

            if (Selection.LastStatus == RunStatus.Failure)
            {
                yield return RunStatus.Failure;
                yield break;
            }

            yield return RunStatus.Success;
            yield break;
        }
    }

    public class SwitchArgument<T>
    {
        public SwitchArgument(Composite branch, T requiredValue = default(T))
        {
            Branch = branch;
            RequiredValue = requiredValue;
        }

        /// <summary>
        ///   A branch of logic that will be executed if this argument is the correct switch.
        /// </summary>
        public Composite Branch { get; set; }

        /// <summary>
        ///   The value required for this logic branch to be executed.
        /// </summary>
        public T RequiredValue { get; set; }
    }
}