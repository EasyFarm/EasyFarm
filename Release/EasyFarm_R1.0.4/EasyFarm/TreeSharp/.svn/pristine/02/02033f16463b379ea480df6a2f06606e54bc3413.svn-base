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

namespace TreeSharp
{
    /// <summary>
    ///   The base class of the entire behavior tree system.
    ///   Nearly all branches derive from this class.
    /// </summary>
    public abstract class Composite : IEquatable<Composite>
    {
        protected static readonly object Locker = new object();

        private IEnumerator<RunStatus> _current;

        protected Composite()
        {
            Guid = Guid.NewGuid();
            CleanupHandlers = new Stack<CleanupHandler>();
        }

        public RunStatus? LastStatus { get; set; }

        protected ContextChangeHandler ContextChanger { get; set; }

        protected Stack<CleanupHandler> CleanupHandlers { get; set; }

        public Composite Parent { get; set; }

        /// <summary>
        ///   Simply an identifier to make sure each composite is 'unique'.
        ///   Useful for XML declaration parsing.
        /// </summary>
        protected Guid Guid { get; set; }

        #region IEquatable<Composite> Members

        /// <summary>
        ///   Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <returns>
        ///   true if the current object is equal to the <paramref name = "other" /> parameter; otherwise, false.
        /// </returns>
        /// <param name = "other">An object to compare with this object.</param>
        public bool Equals(Composite other)
        {
            if (ReferenceEquals(null, other))
            {
                return false;
            }
            if (ReferenceEquals(this, other))
            {
                return true;
            }
            return other.Guid.Equals(Guid);
        }

        #endregion

        /// <summary>
        ///   Determines whether the specified <see cref = "T:System.Object" /> is equal to the current <see cref = "T:System.Object" />.
        /// </summary>
        /// <returns>
        ///   true if the specified <see cref = "T:System.Object" /> is equal to the current <see cref = "T:System.Object" />; otherwise, false.
        /// </returns>
        /// <param name = "obj">The <see cref = "T:System.Object" /> to compare with the current <see cref = "T:System.Object" />. </param>
        /// <filterpriority>2</filterpriority>
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }
            if (ReferenceEquals(this, obj))
            {
                return true;
            }
            if (obj.GetType() != typeof(Composite))
            {
                return false;
            }
            return Equals((Composite)obj);
        }

        /// <summary>
        ///   Serves as a hash function for a particular type.
        /// </summary>
        /// <returns>
        ///   A hash code for the current <see cref = "T:System.Object" />.
        /// </returns>
        /// <filterpriority>2</filterpriority>
        public override int GetHashCode()
        {
            return Guid.GetHashCode();
        }

        public static bool operator ==(Composite left, Composite right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(Composite left, Composite right)
        {
            return !Equals(left, right);
        }

        public abstract IEnumerable<RunStatus> Execute(object context);

        public RunStatus Tick(object context)
        {
            lock (Locker)
            {
                if (LastStatus.HasValue && LastStatus != RunStatus.Running)
                {
                    return LastStatus.Value;
                }
                if (_current == null)
                {
                    throw new ApplicationException("Cannot run Tick before running Start first!");
                }
                if (_current.MoveNext())
                {
                    LastStatus = _current.Current;
                }
                else
                {
                    throw new ApplicationException("Nothing to run? Somethings gone terribly, terribly wrong!");
                }

                if (LastStatus != RunStatus.Running)
                {
                    Stop(context);
                }

                return LastStatus.Value;
            }
        }

        public virtual void Start(object context)
        {
            LastStatus = null;
            _current = Execute(context).GetEnumerator();
        }

        public virtual void Stop(object context)
        {
            Cleanup();
            if (_current != null)
            {
                _current.Dispose();
                _current = null;
            }

            if (LastStatus.HasValue && LastStatus.Value == RunStatus.Running)
            {
                LastStatus = RunStatus.Failure;
            }
        }

        protected void Cleanup()
        {
            if (CleanupHandlers.Count != 0)
            {
                lock (Locker)
                {
                    while (CleanupHandlers.Count != 0)
                    {
                        CleanupHandlers.Pop().Dispose();
                    }
                }
            }
        }

        #region Nested type: CleanupHandler

        protected abstract class CleanupHandler : IDisposable
        {
            protected CleanupHandler(Composite owner, object context)
            {
                Owner = owner;
                Context = context;
            }

            protected Composite Owner { get; set; }

            private object Context { get; set; }

            private bool IsDisposed { get; set; }

            #region IDisposable Members

            public void Dispose()
            {
                if (!IsDisposed)
                {
                    lock (Locker)
                    {
                        IsDisposed = true;
                        DoCleanup(Context);
                    }
                }
            }

            #endregion

            protected abstract void DoCleanup(object context);
        }

        #endregion
    }
}