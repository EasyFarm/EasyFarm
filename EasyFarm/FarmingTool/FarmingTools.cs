using EasyFarm.State;
using EasyFarm.UserSettings;
using FFACETools;
using System;
using ZeroLimits.XITools;

namespace ZeroLimits.FarmingTool
{
    public class FarmingTools
    {
        /// <summary>
        /// Singleton instance of the current farming tools.
        /// </summary>
        private static FarmingTools _farmingTools;

        /// <summary>
        /// The current fface instance bound to farming tools. 
        /// </summary>
        private static FFACE _fface;

        private FarmingTools(FFACE fface)
        {
            _fface = fface;
            this.AbilityExecutor = new AbilityExecutor(fface);
            this.AbilityService = new AbilityService();
            this.CombatService = new CombatService(fface);
            this.RestingService = new RestingService(fface);
            this.UnitService = new UnitService(fface);
            this.PlayerData = new PlayerData(fface);
            this.TargetData = new TargetData(fface);
            this.PlayerActions = new PlayerActions(fface);
            this.ActionBlocked = new ActionBlocked(fface);
            this.UserSettings = new Config();
        }

        /// <summary>
        /// A single point of access method that returns a FarmingTools object. 
        /// Make sure you have set an FFACE object before calling this method. 
        /// </summary>
        /// <returns></returns>
        public static FarmingTools GetInstance()
        {
            return _farmingTools;
        }

        /// <summary>
        /// A single point of access method that returns a FarmingTools object.
        /// The object returned will be based on the FFACE instance provided or
        /// if no object was previously created, it will create one for you. 
        /// </summary>
        /// <param name="fface"></param>
        /// <returns></returns>
        public static FarmingTools GetInstance(FFACE fface)
        {
            if (_farmingTools == null || !_fface.Equals(fface))
            {
                _farmingTools = new FarmingTools(fface);
            }

            return _farmingTools;
        }

        /// <summary>
        /// Provides access to FFACE memory reading api which returns details
        /// about various game environment objects. 
        /// </summary>
        public FFACE FFACE
        {
            get { return _fface; }
            set { _fface = value; }
        }

        /// <summary>
        /// Provides services for acquiring ability/spell data.
        /// </summary>
        public AbilityService AbilityService { get; set; }

        /// <summary>
        /// Provides the ability to executor abilities/spells.
        /// </summary>
        public AbilityExecutor AbilityExecutor { get; set; }

        /// <summary>
        /// Provides methods for performing battle.
        /// </summary>
        public CombatService CombatService { get; set; }

        /// <summary>
        /// Provides methods for resting our character.
        /// </summary>
        public RestingService RestingService { get; set; }

        /// <summary>
        /// Provide details about the units around us. 
        /// </summary>
        public UnitService UnitService { get; set; }

        /// <summary>
        /// Provides details about our player.
        /// </summary>
        public PlayerData PlayerData { get; set; }

        /// <summary>
        /// Provides details about our target.
        /// </summary>
        public TargetData TargetData { get; set; }

        /// <summary>
        /// Provides details about player's usable abilities/spells.
        /// </summary>
        public PlayerActions PlayerActions { get; set; }

        /// <summary>
        /// Provides methods on whether an ability/spell is usable.
        /// </summary>
        public ActionBlocked ActionBlocked { get; set; }

        /// <summary>
        /// Provides a central storage place of information needed by the GUI
        /// or other objects in this file.
        /// </summary>
        public Config UserSettings { get; set; }

        /// <summary>
        /// Saves the settings of Config object to file for later retrieval.
        /// </summary>
        /// <param name="Engine"></param>
        public void SaveSettings()
        {
            String Filename = FFACE.Player.Name + "_UserPref.xml";
            Serialization.Serialize(Filename, UserSettings);
        }

        /// <summary>
        /// Loads the settings from the player specific configuration file to the Config obj.
        /// </summary>
        public void LoadSettings()
        {
            String Filename = FFACE.Player.Name + "_UserPref.xml";
            UserSettings = Serialization.Deserialize(Filename, UserSettings);
        }
    }
}
