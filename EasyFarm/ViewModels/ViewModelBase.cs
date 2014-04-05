using EasyFarm.Classes;
using MvvmFoundation.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EasyFarm.ViewModels
{
    public class ViewModelBase : ObservableObject
    {
        protected GameEngine GameEngine;              

        protected ViewModelBase(ref GameEngine Engine)
        {
            this.GameEngine = Engine;
        }
    }
}
