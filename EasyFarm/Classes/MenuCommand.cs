using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using EliteMMO.API;
using MemoryAPI;

namespace EasyFarm.Classes
{
    public class MenuCommand
    {
        public MenuCommand(string command)
        {
            if (command.StartsWith("Target"))
            {
                var split = command
                    .Split(new[] { @"""" }, StringSplitOptions.RemoveEmptyEntries)
                    .Select(x => x.Trim())
                    .ToList();

                CommandType = split[0];
                Target = split[1];
            }

            if (command.StartsWith("Expect"))
            {
                var split = command
                    .Split(new[] { @"""" }, StringSplitOptions.RemoveEmptyEntries)
                    .Select(x => x.Trim())
                    .ToList();

                CommandType = split[0];
                Expect = split[1];
            }

            if (command.StartsWith("Wait"))
            {
                var split = command
                    .Split(new[] { " " }, StringSplitOptions.RemoveEmptyEntries)
                    .Select(x => x.Trim())
                    .ToList();

                CommandType = split[0];
                Delay = TimeSpan.FromSeconds(double.Parse(split[1]));
            }

            if (command.StartsWith("Key"))
            {
                var split = command
                    .Split(new[] { " " }, StringSplitOptions.RemoveEmptyEntries)
                    .Select(x => x.Trim())
                    .ToList();

                CommandType = split[0];
                var key = split[1];

                if (key.ToLowerInvariant() == "enter")
                    Key = Keys.NUMPADENTER;
                if (key.ToLowerInvariant() == "escape")
                    Key = Keys.ESCAPE;
                if (key.ToLowerInvariant() == "up")
                    Key = Keys.UP;
                if (key.ToLowerInvariant() == "down")
                    Key = Keys.DOWN;
                if (key.ToLowerInvariant() == "left")
                    Key = Keys.LEFT;
                if (key.ToLowerInvariant() == "right")
                    Key = Keys.RIGHT;
            }
        }

        public string CommandType { get; set; }
        public string Target { get; set; }
        public TimeSpan Delay { get; set; }
        public Keys Key { get; set; }
        public string Expect { get; set; }

        public void Run(IMemoryAPI memoryApi)
        {
            if (CommandType.StartsWith("Target"))
            {
                var target = FindTarget(memoryApi);
                if (target == null) return;
                ApproachTarget(memoryApi, target);
            }

            if (CommandType.StartsWith("Wait"))
            {
                Thread.Sleep((int)Delay.TotalMilliseconds);
            }

            if (CommandType.StartsWith("Key") && Key != 0)
            {
                memoryApi.Windower.SendKeyPress(Key);
            }
        }

        private IUnit FindTarget(IMemoryAPI memoryApi)
        {
            var units = new UnitService(memoryApi);
            var target = UnitService.Units.FirstOrDefault(x =>
                string.Equals(x.Name, Target,
                    StringComparison.InvariantCultureIgnoreCase));
            return target;
        }

        private static void ApproachTarget(IMemoryAPI memoryApi, IUnit target)
        {
            var preserveTolerance = memoryApi.Navigator.DistanceTolerance;
            memoryApi.Navigator.DistanceTolerance = 2.0;

            memoryApi.Navigator.GotoWaypoint(target.Position, false, false);
            memoryApi.Target.SetNPCTarget(target.Id);

            memoryApi.Navigator.DistanceTolerance = preserveTolerance;
        }
    }
}