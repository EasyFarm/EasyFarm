using System;
using MediatR;

namespace EasyFarm.Infrastructure
{
    public class NavigateViewRequest : IRequest
    {
        public Type Type { get; }

        public NavigateViewRequest(Type type)
        {
            Type = type;
        }

        public static NavigateViewRequest Create<TViewModel>()
        {
            return new NavigateViewRequest(typeof(TViewModel));
        }
    }
}