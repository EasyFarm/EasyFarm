using System.Collections.ObjectModel;
using EasyFarm.Classes;
using EasyFarm.States;

namespace EasyFarm.UserSettings
{
    /// <summary>
    /// Control direct access to the global config instance in a single place.
    /// </summary>
    /// <remarks>
    /// Purely to enable testing and prevent tests from interacting with each other through global state.
    /// </remarks>
    public class ProxyConfig : IConfig
    {
        public bool AggroFilter
        {
            get => Config.Instance.AggroFilter;
            set => Config.Instance.AggroFilter = value;
        }
        public BattleLists BattleLists
        {
            get => Config.Instance.BattleLists;
            set => Config.Instance.BattleLists = value;
        }
        public bool ClaimedFilter
        {
            get => Config.Instance.ClaimedFilter;
            set => Config.Instance.ClaimedFilter = value;
        }
        public double DetectionDistance
        {
            get => Config.Instance.DetectionDistance;
            set => Config.Instance.DetectionDistance = value;
        }
        public int GlobalCooldown
        {
            get => Config.Instance.GlobalCooldown;
            set => Config.Instance.GlobalCooldown = value;
        }
        public double HeightThreshold
        {
            get => Config.Instance.HeightThreshold;
            set => Config.Instance.HeightThreshold = value;
        }
        public int HighHealth
        {
            get => Config.Instance.HighHealth;
            set => Config.Instance.HighHealth = value;
        }
        public int HighMagic
        {
            get => Config.Instance.HighMagic;
            set => Config.Instance.HighMagic = value;
        }
        public ObservableCollection<string> IgnoredMobs
        {
            get => Config.Instance.IgnoredMobs;
            set => Config.Instance.IgnoredMobs = value;
        }
        public string IgnoredName
        {
            get => Config.Instance.IgnoredName;
            set => Config.Instance.IgnoredName = value;
        }
        public bool IsApproachEnabled
        {
            get => Config.Instance.IsApproachEnabled;
            set => Config.Instance.IsApproachEnabled = value;
        }
        public bool IsEngageEnabled
        {
            get => Config.Instance.IsEngageEnabled;
            set => Config.Instance.IsEngageEnabled = value;
        }
        public bool IsHealthEnabled
        {
            get => Config.Instance.IsHealthEnabled;
            set => Config.Instance.IsHealthEnabled = value;
        }
        public bool IsMagicEnabled
        {
            get => Config.Instance.IsMagicEnabled;
            set => Config.Instance.IsMagicEnabled = value;
        }
        public int LowHealth
        {
            get => Config.Instance.LowHealth;
            set => Config.Instance.LowHealth = value;
        }
        public int LowMagic
        {
            get => Config.Instance.LowMagic;
            set => Config.Instance.LowMagic = value;
        }
        public double MeleeDistance
        {
            get => Config.Instance.MeleeDistance;
            set => Config.Instance.MeleeDistance = value;
        }
        public bool PartyFilter
        {
            get => Config.Instance.PartyFilter;
            set => Config.Instance.PartyFilter = value;
        }
        public ObservableCollection<string> TargetedMobs
        {
            get => Config.Instance.TargetedMobs;
            set => Config.Instance.TargetedMobs = value;
        }
        public string TargetName
        {
            get => Config.Instance.TargetName;
            set => Config.Instance.TargetName = value;
        }
        public bool UnclaimedFilter
        {
            get => Config.Instance.UnclaimedFilter;
            set => Config.Instance.UnclaimedFilter = value;
        }
        public double WanderDistance
        {
            get => Config.Instance.WanderDistance;
            set => Config.Instance.WanderDistance = value;
        }
        public bool StraightRoute
        {
            get => Config.Instance.Route.StraightRoute;
            set => Config.Instance.Route.StraightRoute = value;
        }
        public bool MinimizeToTray
        {
            get => Config.Instance.MinimizeToTray;
            set => Config.Instance.MinimizeToTray = value;
        }
        public int TrustPartySize
        {
            get => Config.Instance.TrustPartySize;
            set => Config.Instance.TrustPartySize = value;
        }
        public bool HomePointOnDeath
        {
            get => Config.Instance.HomePointOnDeath;
            set => Config.Instance.HomePointOnDeath = value;
        }
        public bool EnableTabTargeting
        {
            get => Config.Instance.EnableTabTargeting;
            set => Config.Instance.EnableTabTargeting = value;
        }
        public bool IsObjectAvoidanceEnabled
        {
            get => Config.Instance.IsObjectAvoidanceEnabled;
            set => Config.Instance.IsObjectAvoidanceEnabled = value;
        }
        public double FollowDistance
        {
            get => Config.Instance.FollowDistance;
            set => Config.Instance.FollowDistance = value;
        }
        public string FollowedPlayer
        {
            get => Config.Instance.FollowedPlayer;
            set => Config.Instance.FollowedPlayer = value;
        }

        public Route Route
        {
            get => Config.Instance.Route;
            set => Config.Instance.Route = value;
        }
    }
}