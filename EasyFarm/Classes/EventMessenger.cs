using System;
using System.Collections.Generic;

namespace EasyFarm.Classes
{
    public class EventMessenger
    {
        private readonly List<Action<Object>> _handlers = new List<Action<Object>>();

        public void Fire(Object @event)
        {
            _handlers.ForEach(x => x.Invoke(@event));
        }

        public void Bind<T>(Action<T> func)
        {
            _handlers.Add(WrapEvent(func));
        }

        private Action<Object> WrapEvent<T>(Action<T> func)
        {
            return (e) =>
            {
                if (e.GetType() == typeof(T))
                {
                    func.Invoke((T)e);
                };
            };
        }
    }
}