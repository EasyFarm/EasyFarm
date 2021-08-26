using EasyFarm.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyFarm.States
{
    class DebugNavigationState : BaseState
    {
        public override bool Check(IGameContext context)
        {
            return true;
        }

        public override void Run(IGameContext context)
        {
            context.API.Navigator.DistanceTolerance = 1;

            var currentPosition = context.Config.Route.GetCurrentPosition(context.API.Player.Position);
            if (currentPosition == null || currentPosition.Distance(context.API.Player.Position) <= 0.5)
            {
                currentPosition = context.Config.Route.GetNextPosition(context.API.Player.Position);
            }

            var path = context.NavMesh.FindPathBetween(context.API.Player.Position, currentPosition);
            if (path.Count > 0)
            {
                context.API.Navigator.DistanceTolerance = 0.5;

                while (path.Count > 0 && path.Peek().Distance(context.API.Player.Position) <= context.API.Navigator.DistanceTolerance)
                {
                    path.Dequeue();
                }

                if (path.Count > 0)
                {
                    var node = path.Peek();

                    float deltaX = node.X - context.API.Player.Position.X;
                    float deltaY = node.Y - context.API.Player.Position.Y;
                    float deltaZ = node.Z - context.API.Player.Position.Z;
                    context.API.Follow.SetFollowCoords(deltaX, deltaY, deltaZ);
                }
                else
                {
                    context.Config.Route.GetNextPosition(context.API.Player.Position);
                }
            }
        }

        public override void Exit(IGameContext context)
        {
            context.API.Follow.Reset();
        }
    }
}
