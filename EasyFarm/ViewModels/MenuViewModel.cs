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

using System.Windows.Input;
using EasyFarm.Classes;
using EasyFarm.Infrastructure;
using Prism.Commands;

namespace EasyFarm.ViewModels
{
    public class MenuViewModel : ViewModelBase
    {
        public MenuViewModel()
        {
            ViewName = "Menu";
            SaveCommand = new DelegateCommand(Save);
        }

        public string Commands
        {
            get { return Config.Instance.MenuCommands; }
            set
            {
                var commands = "";
                SetProperty(ref commands, value);
                Config.Instance.MenuCommands = commands;
            }
        }

        public ICommand SaveCommand { get; set; }

        private void Save()
        {
        }
    }
}