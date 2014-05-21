
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

using EasyFarm.Classes;
using Microsoft.Practices.Prism.Mvvm;
using Microsoft.Practices.Prism.PubSubEvents;
using Microsoft.Practices.Prism.ViewModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows.Controls;

namespace EasyFarm.ViewModels
{
    public class ViewModelBase : BindableBase
    {
        protected GameEngine _engine;
        protected IEventAggregator eventAggregator;

        protected ViewModelBase(ref GameEngine Engine, IEventAggregator eventAggregator)
        {
            this.eventAggregator = eventAggregator;
            this._engine = Engine;
        }

        /// <summary>
        /// Update the user on what's happening.
        /// </summary>
        /// <param name="message">The message to display in the statusbar</param>
        public void InformUser(String message)
        {
            eventAggregator.GetEvent<StatusBarUpdateEvent>().Publish(message);
        }
    }
}
