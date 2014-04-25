using EasyFarm.Classes;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyFarm.Models
{
    public class BattleModel
    {
        /// <summary>
        /// A player's battle moves.
        /// </summary>
        public ObservableCollection<Ability> BattleMoves;

        /// <summary>
        /// A player's before-battle moves.
        /// </summary>
        public ObservableCollection<Ability> StartMoves;

        /// <summary>
        /// A player's after-battle moves.
        /// </summary>
        public ObservableCollection<Ability> EndMoves;
    }
}
