// Author: atom0s
// URL: https://github.com/atom0s/AshitaLauncher
// Modified by Zerolimits to add a serializable dictionary for easier saving of properties to file.

/**
 * NotifiableModel.cs - Base view model class for easy property implementation.
 * -----------------------------------------------------------------------
 * 
 *		This file is part of Ashita.
 *
 *		Ashita is free software: you can redistribute it and/or modify
 *		it under the terms of the GNU Lesser General Public License as published by
 *		the Free Software Foundation, either version 3 of the License, or
 *		(at your option) any later version.
 *
 *		Ashita is distributed in the hope that it will be useful,
 *		but WITHOUT ANY WARRANTY; without even the implied warranty of
 *		MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 *		GNU Lesser General Public License for more details.
 *
 *		You should have received a copy of the GNU Lesser General Public License
 *		along with Ashita.  If not, see <http://www.gnu.org/licenses/>.
 *
 */

namespace Ashita.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;

    /// <summary>
    /// Notifiable model implementation that wraps the INotifyPropertyChanged
    /// class for easier usage. Makes property get/set methods cleaner.
    /// </summary>
    public class NotifiableModel : INotifyPropertyChanged
    {
        /// <summary>
        /// Internal properties container.
        /// </summary>
        private readonly SerializableDictionary<String, Object> _properties;

        /// <summary>
        /// Default Constructor
        /// </summary>
        public NotifiableModel()
        {
            this._properties = new SerializableDictionary<String, Object>();
        }

        /// <summary>
        /// Event triggered when a property is changed.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Method used to raise the PropertyChanged event.
        /// </summary>
        /// <param name="prop"></param>
        public void OnPropertyChanged(String prop)
        {
            if (this.PropertyChanged != null)
                this.PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }

        /// <summary>
        /// Method to raise the PropertyChanged event.
        /// </summary>
        /// <param name="property"></param>
        protected void RaisePropertyChanged(string property)
        {
            if (string.IsNullOrEmpty(property))
                throw new ArgumentNullException(property);
            this.OnPropertyChanged(property);
        }

        /// <summary>
        /// Gets a property from the internal container.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="prop"></param>
        /// <returns></returns>
        protected T Get<T>(String prop)
        {
            if (this._properties.ContainsKey(prop))
            {
                return (T)this._properties[prop];
            }
            return default(T);
        }

        /// <summary>
        /// Sets a property in the internal container.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="prop"></param>
        /// <param name="val"></param>
        protected void Set<T>(String prop, T val)
        {
            var curr = this.Get<T>(prop);
            if (Equals(curr, val))
                return;

            this._properties[prop] = val;
            this.OnPropertyChanged(prop);
        }
    }
}
