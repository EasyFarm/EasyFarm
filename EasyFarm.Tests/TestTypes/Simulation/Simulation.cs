using System;
using System.Collections.Generic;

namespace EasyFarm.Tests.TestTypes.Simulation
{
    public class Simulation : ISimulation
    {
        private readonly Dictionary<string, object> _state = new Dictionary<string, object>();

        public void Run()
        {
            _state.Add("Player.Stats.Str", (short)100);
        }

        public T GetState<T>(string key)
        {
            _state.TryGetValue(key, out object value);
            if (value == null) throw new InvalidOperationException($"State is not defined for {key}.");
            return (T) value;
        }
    }
}