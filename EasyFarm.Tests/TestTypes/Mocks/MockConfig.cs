using System.Collections.ObjectModel;
using EasyFarm.Classes;
using EasyFarm.States;
using EasyFarm.UserSettings;

namespace EasyFarm.Tests.TestTypes.Mocks
{
    public class MockConfig : IConfig
    {
        public bool AggroFilter { get; set; }
        public BattleLists BattleLists { get; set; } = new BattleLists();
        public bool ClaimedFilter { get; set; }
        public double DetectionDistance { get; set; }
        public int GlobalCooldown { get; set; }
        public double HeightThreshold { get; set; }
        public int HighHealth { get; set; }
        public int HighMagic { get; set; }
        public ObservableCollection<string> IgnoredMobs { get; set; } = new ObservableCollection<string>();
        public string IgnoredName { get; set; }
        public bool IsApproachEnabled { get; set; }
        public bool IsEngageEnabled { get; set; }
        public bool IsHealthEnabled { get; set; }
        public bool IsMagicEnabled { get; set; }
        public int LowHealth { get; set; }
        public int LowMagic { get; set; }
        public double MeleeDistance { get; set; }
        public bool PartyFilter { get; set; }
        public ObservableCollection<string> TargetedMobs { get; set; } = new ObservableCollection<string>();
        public string TargetName { get; set; }
        public bool UnclaimedFilter { get; set; }
        public double WanderDistance { get; set; }
        public bool StraightRoute { get; set; }
        public bool MinimizeToTray { get; set; }
        public int TrustPartySize { get; set; }
        public bool HomePointOnDeath { get; set; }
        public bool EnableTabTargeting { get; set; }
        public bool IsObjectAvoidanceEnabled { get; set; }
        public double FollowDistance { get; set; }
        public string FollowedPlayer { get; set; }
        public Route Route { get; set; } = new Route();

        public MockConfig()
        {
            BattleLists = new BattleLists(Config.Instance.BattleLists);
        }
    }
}