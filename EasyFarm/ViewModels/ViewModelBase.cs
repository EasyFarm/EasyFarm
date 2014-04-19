using EasyFarm.Classes;
using Microsoft.Practices.Prism.Mvvm;
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
        protected GameEngine GameEngine;

        protected ViewModelBase(ref GameEngine Engine)
        {
            this.GameEngine = Engine;
        }
    }
}
