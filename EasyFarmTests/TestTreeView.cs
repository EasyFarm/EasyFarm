using System.Collections.Generic;
using EasyFarm.Classes;
using EasyFarm.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Parsing.Services;

namespace EasyFarmTests
{
    [TestClass]
    public class TestTreeView
    {
        /// <summary>
        ///     Retrieves abilities from resource files.
        /// </summary>
        private static readonly AbilityService retriever;

        /// <summary>
        ///     Set up the ability retriever.
        /// </summary>
        static TestTreeView()
        {
            retriever = new AbilityService("resources");
        }

        [TestMethod]
        public void TreeViewLayering()
        {
            // Create the TreeItemViewModels that will be stored in layer one. 
            var Fire = CreateTreeItem("Fire");
            var Water = CreateTreeItem("Water");
            var Aero = CreateTreeItem("Aero");

            // Create a layer of tree view items. 
            var Layer1 = CreateTreeLayer("Layer1", Fire, Water, Aero);
            var Layer2 = CreateTreeLayer("Layer2", Fire, Water, Aero);
            var Layer3 = CreateTreeLayer("Layer3", Layer1, Layer2);
        }

        public TreeItemViewModel<IEnumerable<TreeItemViewModel<T>>>
            CreateTreeLayer<T>(
            string name,
            params TreeItemViewModel<T>[] items)
        {
            return new TreeItemViewModel<IEnumerable<TreeItemViewModel<T>>>(name, items);
        }

        public TreeItemViewModel<BattleAbility> CreateTreeItem(string name)
        {
            var ability = new BattleAbility(retriever.CreateAbility(name));
            return new TreeItemViewModel<BattleAbility>(ability.Name, ability);
        }
    }
}