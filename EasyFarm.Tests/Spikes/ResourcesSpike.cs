using System;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using EasyFarm.Parsing;
using EasyFarm.Tests.Parsing;
using EliteMMO.API;
using Xunit;

namespace EasyFarm.Tests.Spikes
{
    public class ResourcesSpike
    {
        public void SpellMapper_CreatesUsableAbility()
        {
            // Setup fixture
            EliteAPI api = CreateAPI();
            var mapper = new SpellMapper();
            // Exercise system
            var spell = api.Resources.GetSpell("Cure", 0);
            var resource = mapper.Map(spell);
            Int32 recast = SendSpell(api, resource, spell);
            // Verify outcome
            Assert.NotEqual(0, recast);
            // Teardown
        }

        public void AbllityMapper_CreatesUsableAbility()
        {
            // Setup fixture
            EliteAPI api = CreateAPI();
            var mapper = new AbilityMapper();
            // Exercise system
            var ability = api.Resources.GetAbility("Afflatus Solace", 0);
            var resource = mapper.Map(ability);
            Int32 recast = SendAbility(api, resource, ability);
            // Verify outcome
            Assert.NotEqual(0, recast);
            // Teardown
        }

        public void ItemMapper_CreatesUsableAbility()
        {
            // Setup fixture
            EliteAPI api = CreateAPI();
            var mapper = new ItemMapper();
            // Exercise system
            var item = api.Resources.GetItem("Meat Jerky", 0);
            var resource = mapper.Map(item);
            Int32 recast = SendItem(api, resource, item);
            // Verify outcome
            Assert.NotEqual(0, recast);
            // Teardown
        }

        private static Int32 SendSpell(EliteAPI api, Ability resource, EliteAPI.ISpell spell)
        {
            api.ThirdParty.SendString(resource.Command);
            Thread.Sleep(500);

            // ReSharper disable once CompareOfFloatsByEqualityOperator
            while (api.CastBar.Percent == 0)
            {
            }

            Thread.Sleep(500);

            while (api.CastBar.Percent != 1.0)
            {
            }

            Thread.Sleep(500);

            var recast = api.Recast.GetSpellRecast(spell.Index);
            return recast;
        }

        private static Int32 SendAbility(EliteAPI api, Ability resource, EliteAPI.IAbility ability)
        {
            api.ThirdParty.SendString(resource.Command);
            Thread.Sleep(1000);

            var recastIds = api.Recast.GetAbilityIds();
            var index = recastIds.IndexOf(resource.Index);
            var recast = api.Recast.GetAbilityRecast(index);
            return recast;
        }

        private static Int32 SendItem(EliteAPI api, Ability resource, EliteAPI.IItem ability)
        {
            api.ThirdParty.SendString(resource.Command);
            Thread.Sleep(1000);
            return 0;
        }


        private static EliteAPI CreateAPI()
        {
            var process = Process.GetProcessesByName("pol").FirstOrDefault();
            var api = new EliteAPI(process.Id);
            return api;
        }
    }
}
