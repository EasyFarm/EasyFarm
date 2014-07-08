using FFACETools;
using Microsoft.Practices.Prism.PubSubEvents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyFarm.Classes.Services
{
    public class FarmingTools
    {
        private static FarmingTools _farmingTools;
        private static FFACE _fface;

        public FarmingTools(FFACE fface)
        {
            _fface = fface;
            this.AbilityExecutor = new AbilityExecutor(fface);
            this.AbilityService = new AbilityService();
            this.CombatService = new CombatService(fface);
            this.GameEngine = new GameEngine(fface);
            this.RestingService = new RestingService(fface);
            this.UnitService = new UnitService(fface);
            this.PlayerData = new PlayerData(fface);
            this.TargetData = new TargetData(fface);
            this.PlayerActions = new PlayerActions(fface);
            this.ActionBlocked = new ActionBlocked(fface);
        }

        public static FarmingTools GetInstance()
        {
            return _farmingTools;
        }

        public static FarmingTools GetInstance(FFACE fface)
        {
            if (_farmingTools == null || !_fface.Equals(fface)) {
                _farmingTools = new FarmingTools(fface); 
            }

            return _farmingTools;
        }

        public FFACE FFACE 
        {
            get { return _fface; }
            set { _fface = value; }
        }

        public AbilityService AbilityService { get; set; }

        public AbilityExecutor AbilityExecutor { get; set; }

        public CombatService CombatService { get; set; }

        public GameEngine GameEngine { get; set; }

        public RestingService RestingService { get; set; }

        public UnitService UnitService { get; set; }

        public FiniteStateEngine FiniteStateMachine { get; set; }

        public PlayerData PlayerData { get; set; }

        public TargetData TargetData { get; set; }

        public PlayerActions PlayerActions { get; set; }

        public ActionBlocked ActionBlocked { get; set; }

        public Config UserSettings { get; set; }       

        /// <summary>
        /// Saves the settings of Config object to file for later retrieval.
        /// </summary>
        /// <param name="Engine"></param>
        public void SaveSettings()
        {
            UserSettings.FilterInfo = UnitService.FilterInfo;
            String Filename = FFACE.Player.Name + "_UserPref.xml";
            Utilities.Serialize(Filename, UserSettings);
        }

        /// <summary>
        /// Loads the settings from the player specific configuration file to the Config obj.
        /// </summary>
        public void LoadSettings()
        {
            String Filename = FFACE.Player.Name + "_UserPref.xml";
            UserSettings = Utilities.Deserialize(Filename, UserSettings);
            UnitService.FilterInfo = UserSettings.FilterInfo;
        }
    }
}
