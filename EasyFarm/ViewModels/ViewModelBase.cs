using EasyFarm.Classes;
using Microsoft.Practices.Prism.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EasyFarm.ViewModels
{
    public class ViewModelBase : NotificationObject
    {
        protected GameEngine GameEngine;              

        protected ViewModelBase(ref GameEngine Engine)
        {
            this.GameEngine = Engine;
        }
    }
}
