using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parsing.Mapping
{
    /// This class is thanks to these two great people. The only thing I've done is 
    /// patchwork the two methods together. 
    /// 
    /// Author: James Michael Hare
    /// Source: http://geekswithblogs.net/BlackRabbitCoder/archive/2010/12/09/c.net-little-wonders-fun-with-enum-methods.aspx
    /// Author: Julien Lebosquain 
    /// Source: http://stackoverflow.com/questions/79126/create-generic-method-constraining-t-to-an-enum

    public abstract class EnumClassUtils<TClass>
    {
        /// <summary>
        /// Combine generic enums together.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="flags"></param>
        /// <returns></returns>        
        public static TType SetFlag<TType>(TType value, TType flags)
        {
            if (!value.GetType().IsEquivalentTo(typeof(TType)))
            {
                throw new ArgumentException("Failed to combine enums generically. ");
            }

            return (TType)Enum.ToObject(typeof(TType), Convert.ToUInt64(value) | Convert.ToUInt64(flags));
        }
    }
}
