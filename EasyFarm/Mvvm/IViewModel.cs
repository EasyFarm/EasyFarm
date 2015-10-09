using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyFarm.Mvvm
{
    public interface IViewModel
    {
        /// <summary>
        /// The name of the view model. 
        /// </summary>
        string ViewName { get; set; }
    }
}
