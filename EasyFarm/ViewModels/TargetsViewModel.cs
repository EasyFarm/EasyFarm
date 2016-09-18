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

using System.Collections.ObjectModel;
using EasyFarm.Classes;

namespace EasyFarm.ViewModels
{
    public class TargetsViewModel : ListViewModel<string>
    {
        public TargetsViewModel()
        {
            ViewName = "Targets";
        }

        public override string Value
        {
            get { return Config.Instance.TargetName; }
            set { SetProperty(ref Config.Instance.TargetName, value); }
        }

        public override ObservableCollection<string> Values
        {
            get { return Config.Instance.TargetedMobs; }
            set { SetProperty(ref Config.Instance.TargetedMobs, value); }
        }

        public bool Aggro
        {
            get { return Config.Instance.AggroFilter; }
            set { SetProperty(ref Config.Instance.AggroFilter, value); }
        }

        public bool Unclaimed
        {
            get { return Config.Instance.UnclaimedFilter; }
            set { SetProperty(ref Config.Instance.UnclaimedFilter, value); }
        }

        public bool PartyClaimed
        {
            get { return Config.Instance.PartyFilter; }
            set { SetProperty(ref Config.Instance.PartyFilter, value); }
        }

        public bool Claimed
        {
            get { return Config.Instance.ClaimedFilter; }
            set { SetProperty(ref Config.Instance.ClaimedFilter, value); }
        }

        protected override void Add()
        {
            if(string.IsNullOrWhiteSpace(Value)) return;            
            base.Add();
            Value = "";
        }

        protected override void Clear()
        {
            base.Clear();
            Value = "";
        }
    }
}