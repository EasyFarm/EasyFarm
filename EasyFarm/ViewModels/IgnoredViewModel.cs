/*///////////////////////////////////////////////////////////////////
<EasyFarm, general farming utility for FFXI.>
Copyright (C) <2013>  <Zerolimits>

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
using System.Collections.ObjectModel;
using EasyFarm.Infrastructure;

namespace EasyFarm.ViewModels
{
    [ViewModel("Ignored")]
    public class IgnoredViewModel : ListViewModel<string>
    {
        public override string Value
        {
            get { return Config.Instance.IgnoredName; }
            set { SetProperty(ref Config.Instance.IgnoredName, value); }
        }

        public override ObservableCollection<string> Values
        {
            get { return Config.Instance.IgnoredMobs; }
            set { SetProperty(ref Config.Instance.IgnoredMobs, value); }
        }

        /// <summary>
        /// Overload for excluding blanks strings from being added. 
        /// </summary>
        protected override void Add()
        {
            if (string.IsNullOrWhiteSpace(Value)) return;
            base.Add();
        }
    }
}