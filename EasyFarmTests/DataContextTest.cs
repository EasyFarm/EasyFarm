using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using EasyFarm.DataContexts;
using System.Linq;
using System.Data.SQLite;
using Action = EasyFarm.DataContexts.Action;
using System.IO;

namespace EasyFarmTests
{
    [TestClass]
    public class DataContextTest
    {
        [TestMethod]
        public void TestHealthOptions()
        {
            var easyfarmDb = new EasyFarmDB();
            var player = easyfarmDb.Players.Create();

            player.Health.Enabled = true;
            player.Health.Low = 50;
            player.Health.High = 100;
            
            // Add player to database. 
            easyfarmDb.Players.Add(player);
            easyfarmDb.SaveChanges();

            Console.WriteLine(player.Id);                                   
        }
    }
}
