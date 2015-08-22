using EasyFarm.DataContexts;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace EasyFarmTests
{
    [TestClass]
    public class DataContextTest
    {
        [TestMethod]
        public void TestPersistence()
        {
            var easyfarmDb = new EasyFarmDB();
            var health = easyfarmDb.Health.Create();

            health.Enabled = true;
            health.Low = 50;
            health.High = 100;

            // Add player to database. 
            easyfarmDb.Health.Add(health);
            easyfarmDb.SaveChanges();

            var id = health.Id;
            Assert.IsTrue(easyfarmDb.Health.Any(x => x.Id == id));
        }
    }
}
