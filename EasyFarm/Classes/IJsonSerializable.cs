using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyFarm.Classes {
    public interface IJsonSerializable {
        void PrepareForSerialization();
        void CompleteDeserialization(); 
    }
}
