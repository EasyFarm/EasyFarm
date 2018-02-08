// ///////////////////////////////////////////////////////////////////
// This file is a part of EasyFarm for Final Fantasy XI
// Copyright (C) 2013-2017 Mykezero
// 
// EasyFarm is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// EasyFarm is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// If not, see <http://www.gnu.org/licenses/>.
// ///////////////////////////////////////////////////////////////////
using EasyFarm.Classes;
using EasyFarm.Infrastructure;
using EasyFarm.UserSettings;

namespace EasyFarm.ViewModels
{
    public class RestingViewModel : ViewModelBase
    {
        public RestingViewModel()
        {
            ViewName = "Resting";
        }

        public int LowHp
        {
            get { return Config.Instance.LowHealth; }
            set
            {
                Set(ref Config.Instance.LowHealth, value);
                AppServices.InformUser("Low hp set to " + LowHp);
            }
        }

        public int HighHp
        {
            get { return Config.Instance.HighHealth; }
            set
            {
                Set(ref Config.Instance.HighHealth, value);
                AppServices.InformUser("High hp set to " + HighHp);
            }
        }

        public int LowMp
        {
            get { return Config.Instance.LowMagic; }
            set
            {
                Set(ref Config.Instance.LowMagic, value);
                AppServices.InformUser("Low mp set to " + LowMp);
            }
        }

        public int HighMp
        {
            get { return Config.Instance.HighMagic; }
            set
            {
                Set(ref Config.Instance.HighMagic, value);
                AppServices.InformUser("High mp set to " + HighMp);
            }
        }

        public bool HpEnabled
        {
            get { return Config.Instance.IsHealthEnabled; }
            set { Set(ref Config.Instance.IsHealthEnabled, value); }
        }

        public bool MpEnabled
        {
            get { return Config.Instance.IsMagicEnabled; }
            set { Set(ref Config.Instance.IsMagicEnabled, value); }
        }
    }
}