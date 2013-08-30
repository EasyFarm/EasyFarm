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
    public abstract class GroupComposite : Composite
    {
        protected GroupComposite(params Composite[] children)
        {
            Children = new List<Composite>(children);
            foreach (Composite composite in Children)
            {
                if (composite != null)
                {
                    composite.Parent = this;
                }
            }
        }

        public List<Composite> Children { get; set; }

        public Composite Selection { get; protected set; }

        public override void Start(object context)
        {
            CleanupHandlers.Push(new ChildrenCleanupHandler(this, context));
            base.Start(context);
        }

        public void AddChild(Composite child)
        {
            if (child != null)
            {
                child.Parent = this;
                Children.Add(child);
            }
        }

        public void InsertChild(int index, Composite child)
        {
            if (child != null)
            {
                child.Parent = this;
                Children.Insert(index, child);
            }
        }

        #region Nested type: ChildrenCleanupHandler

        protected class ChildrenCleanupHandler : CleanupHandler
        {
            public ChildrenCleanupHandler(GroupComposite owner, object context)
                : base(owner, context)
            {
            }

            protected override void DoCleanup(object context)
            {
                foreach (Composite composite in (Owner as GroupComposite).Children)
                {
                    composite.Stop(context);
                }
            }
        }

        #endregion
    }
}