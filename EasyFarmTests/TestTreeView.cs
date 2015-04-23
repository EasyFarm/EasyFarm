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
        private static readonly AbilityService Retriever;

        /// <summary>
        ///     Set up the ability retriever.
        /// </summary>
        static TestTreeView()
        {
            Retriever = new AbilityService("resources");
        }

        [TestMethod]
        public void TreeViewLayering()
        {
            // Create the TreeItemViewModels that will be stored in layer one. 
            var fire = CreateTreeItem("Fire");
            var water = CreateTreeItem("Water");
            var aero = CreateTreeItem("Aero");

            // Create a layer of tree view items. 
            var layer1 = CreateTreeLayer("Layer1", fire, water, aero);
            var layer2 = CreateTreeLayer("Layer2", fire, water, aero);
            CreateTreeLayer("Layer3", layer1, layer2);
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
            var ability = new BattleAbility(Retriever.CreateAbility(name));
            return new TreeItemViewModel<BattleAbility>(ability.Name, ability);
        }
    }
}