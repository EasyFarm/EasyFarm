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

using System;

namespace EasyFarm.UserSettings
{
    public class ConfigFactory : IConfigFactory
    {
        private static Lazy<Config> _lazy = new Lazy<Config>(() => new Config());

        public ConfigFactory()
        {
            Getter = () => _lazy.Value; ;
            Setter = c => _lazy = new Lazy<Config>(() => c);
        }

        public Func<Config> Getter { get; set; }

        public Action<Config> Setter { get; set; }

        public Config Config
        {
            get { return Getter.Invoke(); }
            set { Setter.Invoke(value);}
        }
    }
}