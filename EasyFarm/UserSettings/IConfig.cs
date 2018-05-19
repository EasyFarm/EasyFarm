using System.Collections.ObjectModel;
using EasyFarm.Classes;
using EasyFarm.States;

namespace EasyFarm.UserSettings
{
    public interface IConfig
    {
        bool AggroFilter { get; set; }
        BattleLists BattleLists { get; set; }
        bool ClaimedFilter { get; set; }
        double DetectionDistance { get; set; }
        int GlobalCooldown { get; set; }
        double HeightThreshold { get; set; }
        int HighHealth { get; set; }
        int HighMagic { get; set; }
        ObservableCollection<string> IgnoredMobs { get; set; }
        string IgnoredName { get; set; }
        bool IsApproachEnabled { get; set; }
        bool IsEngageEnabled { get; set; }
        bool IsHealthEnabled { get; set; }
        bool IsMagicEnabled { get; set; }
        int LowHealth { get; set; }
        int LowMagic { get; set; }
        double MeleeDistance { get; set; }
        bool PartyFilter { get; set; }
        ObservableCollection<string> TargetedMobs { get; set; }
        string TargetName { get; set; }
        bool UnclaimedFilter { get; set; }
        double WanderDistance { get; set; }
        bool StraightRoute { get; set; }
        bool MinimizeToTray { get; set; }
        int TrustPartySize { get; set; }
        bool HomePointOnDeath { get; set; }
        bool EnableTabTargeting { get; set; }
        bool IsObjectAvoidanceEnabled { get; set; }
        double FollowDistance { get; set; }
        string FollowedPlayer { get; set; }
        Route Route { get; set; }
    }
}