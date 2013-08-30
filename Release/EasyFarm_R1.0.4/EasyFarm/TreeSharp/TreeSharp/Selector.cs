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
    ///   The base selector class. This will attempt to execute all branches of logic, until one succeeds. 
    ///   This composite will fail only if all branches fail as well.
    /// </summary>
    public abstract class Selector : GroupComposite
    {
        public Selector(params Composite[] children) : base(children)
        {
        }

        public abstract override IEnumerable<RunStatus> Execute(object context);
    }

    public class ProbabilitySelection
    {
        public Composite Branch;

        public double ChanceToExecute;

        public ProbabilitySelection(Composite branch, double chanceToExecute)
        {
            Branch = branch;
            ChanceToExecute = chanceToExecute;
        }
    }

    /// <summary>
    ///   Will execute random branches of logic, until one succeeds. This composite
    ///   will fail only if all branches fail as well.
    /// </summary>
    public class ProbabilitySelector : Selector
    {
        public ProbabilitySelector(params ProbabilitySelection[] children) : base(children.Select(c => c.Branch).ToArray())
        {
            PossibleBranches = children.OrderBy(c => c.ChanceToExecute).ToArray();
            Randomizer = new Random();
        }

        private ProbabilitySelection[] PossibleBranches { get; set; }

        protected Random Randomizer { get; private set; }

        public override IEnumerable<RunStatus> Execute(object context)
        {
            throw new NotImplementedException();
        }
    }
}