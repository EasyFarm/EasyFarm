using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyFarm.Classes
{
    /// <summary>
    /// Class for finding battle lists by name. 
    /// </summary>
    public class BattleLists
    {
        /// <summary>
        /// Contains all the battle lists. 
        /// </summary>
        public ObservableCollection<BattleList> Lists = new ObservableCollection<BattleList>();

        public BattleLists() 
        { 
            
        }

        /// <summary>
        /// Access a battle list by its name. 
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public BattleList this[string index]
        {
            get
            {
                // Find the list item with that name. 
                var list = Lists.Where(x => x.Name.Equals(index)).FirstOrDefault();

                // Throw error if now found. 
                if (list == null) throw new Exception(
                     string.Format("No key {0} in battle lists to get value. ", index)
                 );

                // Return the list matching the name. 
                return list;
            }
            set
            {
                // Find the list item with that name. 
                var list = Lists.Where(x => x.Name.Equals(index)).FirstOrDefault();

                // Throw error when key not found.
                if (list == null) throw new Exception(
                     string.Format("No key {0} in battle lists to set value. ", index)
                 );

                // Remove the old reference to the indexed value. 
                Lists.Remove(list);

                // Add the new indexed value. 
                Lists.Add(value);
            }
        }
    }
}
