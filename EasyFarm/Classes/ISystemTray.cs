using System;

namespace EasyFarm.Classes
{
    public interface ISystemTray
    {
        void ConfigureSystemTray(Action minimizingAction, Action unminimizingAction);
        void Minimize(string title, string text);        
        void Unminimize();        
    }
}