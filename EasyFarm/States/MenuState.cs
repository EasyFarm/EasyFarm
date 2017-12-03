using System;
using System.Collections.Generic;
using System.Linq;
using EasyFarm.Classes;
using MemoryAPI;

namespace EasyFarm.States
{
    public class MenuState : BaseState
    {
        public MenuState(IMemoryAPI fface) : base(fface)
        {
        }

        public override bool Check()
        {
            // We don't have to rest.
            if (new RestState(fface).Check()) return false;

            // We don't have to heal.
            if (new HealingState(fface).Check()) return false;

            // We don't need to summon trusts
            if (new SummonTrustsState(fface).Check()) return false;

            var targetCommand = GetMenuCommands()
                .FirstOrDefault(x => !string.IsNullOrEmpty(x.Target));
            if (targetCommand == null) return false;

            var expectCommand = GetMenuCommands()
                .FirstOrDefault(x => !string.IsNullOrEmpty(x.Expect));
            if (expectCommand == null) return false;

            var units = new UnitService(fface);

            // Target npc is not available in the area.
            var target = UnitService.Units.FirstOrDefault(x => string.Equals(x.Name, targetCommand.Target,
                StringComparison.InvariantCultureIgnoreCase));
            if (target == null) return false;

            // Popped mob is on the battle field.
            var expected = UnitService.Units.FirstOrDefault(x => string.Equals(x.Name, expectCommand.Expect,
                StringComparison.InvariantCultureIgnoreCase));
            if (expected != null) return false;

            return true;
        }

        public override void Run()
        {
            var commands = GetMenuCommands();
            commands.ForEach(x => x.Run(fface));
        }

        private static List<MenuCommand> GetMenuCommands()
        {
            return Config.Instance.MenuCommands
                .Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries)
                .Select(x => new MenuCommand(x))
                .ToList();
        }
    }
}