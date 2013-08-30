using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FFACETools;
using EasyFarm.PlayerTools;
using EasyFarm.Profiles;
using EasyFarm.Interfaces;
using System.Xml.Serialization;

namespace EasyFarm.Engine
{
    /// <summary>
    /// A configuration file for the user to edit through his GUI.
    /// Gives the bot access to allow of his decisions.
    /// </summary>
    public class Config
    {
        [XmlIgnore]
        public Dictionary<ActionType, List<Ability>> Actions { get; set; }

        public List<String> IgnoredList       { get; set; }
        public List<String> TargetsList       { get; set; }
        public FFACE.Position[] Waypoints     { get; set; }
        public WeaponAbility Weaponskill      { get; set; }

        public int RestingValue               { get; set; }
        public int StandUPValue               { get; set; }
        public int WeaponSkillHP              { get; set; }

        public bool BattleAggro               { get; set; }
        public bool BattlePartyClaimed        { get; set; }
        public bool BattleUnclaimed           { get; set; }
        public bool ResummonPet               { get; set; }

        public Config()
        {            
            Actions            = new Dictionary<ActionType, List<Ability>>();
            IgnoredList        = new List<string>();
            TargetsList        = new List<string>();
            Waypoints          = new FFACE.Position[0];
            Weaponskill        = new WeaponAbility();

            RestingValue       = 0;
            StandUPValue       = 0;
            WeaponSkillHP      = 100;

            BattleAggro        = true;
            BattlePartyClaimed = true;
            BattleUnclaimed    = true;
            ResummonPet        = false;

            Actions[ActionType.Battle]  = new List<Ability>();
            Actions[ActionType.Enter]   = new List<Ability>();
            Actions[ActionType.Exit]    = new List<Ability>();
            Actions[ActionType.Healing] = new List<Ability>();
        }
    }

    public enum ActionType
    {
        Enter,
        Battle,
        Healing,
        Exit,
    }
}
