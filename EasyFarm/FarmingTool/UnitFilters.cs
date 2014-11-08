﻿using EasyFarm.UserSettings;
using EasyFarm.ViewModels;
using FFACETools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using ZeroLimits.FarmingTool;
using ZeroLimits.XITool;
using ZeroLimits.XITool.Classes;
using ZeroLimits.XITool.Interfaces;

namespace EasyFarm.FarmingTool
{
    public class UnitFilters
    {
        #region MOBFilter

        /// <summary>
        /// This function should weed out any creatures with bad values.
        /// Aftewards it will check our filtering. 
        ///     If its on the ignored list, ignore it. 
        ///     If its not on the targets list and there is a list, ignore it
        ///     If its on the targets list check
        ///         If the mob is our claim and is checked, attack it.
        ///         if the mob is our partys claimand is checked, attack it.
        ///         if the mob is unclaimed and is checked, attack it.
        ///         if the mob is claimed and not our claimed and 
        ///         claimed checked attack it.
        ///
        /// The function will not attack mobs that have aggroed but
        /// are not on the target's list or 
        /// </summary>
        /// <param name="fface"></param>
        /// <returns></returns>
        public static Func<IUnit, bool> MobFilter(FFACE fface)
        {
            // Function to use to filter surrounding mobs by.
            return new Func<IUnit, bool>((IUnit mob) =>
            {            
                // General Mob Filtering Criteria

                // No fface? Bail. 
                if (fface == null) return false;

                //FIXED: Added null check to avoid certain states
                // secretly throwing null pointer exceptions. 
                // If the mob is null, bail.
                if (mob == null) return false;

                // Mob not active
                if (!mob.IsActive) return false;

                // INFO: fixes trying to attack dead mob problem. 
                // Mob is dead
                if (mob.IsDead) return false;

                // Allow for mobs with an npc bit of sometimes 4 (colibri) 
                // and ignore mobs that are invisible npcbit = 0
                if(Config.Instance.FilterInfo.BitCheck)
                    if (mob.NPCBit >= 16) return false;

                // Type is not mob 
                if (!mob.NPCType.Equals(NPCType.Mob)) return false;

                // Mob is out of range
                if (!(mob.Distance < Config.Instance.MiscSettings.DetectionDistance)) return false;

                // Mob too high out of reach. 
                if (mob.YDifference > Config.Instance.MiscSettings.HeightThreshold) return false;
                
                // User Specific Filtering
               
                // Performs a case insensitve match on the mob's name. 
                // If any part of the mob's name is in the ignored list,
                // we will not attack it. 
                if (MatchAny(mob.Name, Config.Instance.FilterInfo.IgnoredMobs, 
                    RegexOptions.IgnoreCase)) return false;

                // Kill aggro if aggro's checked regardless of target's list but follows 
                // the ignored list. 
                if (mob.HasAggroed && Config.Instance.FilterInfo.AggroFilter) return true;

                // There is a target's list but the mob is not on it. 
                if (!MatchAny(mob.Name, Config.Instance.FilterInfo.TargetedMobs,RegexOptions.IgnoreCase) && 
                    Config.Instance.FilterInfo.TargetedMobs.Any()) 
                    return false;

                // Mob on our targets list.
                if (MatchAny(mob.Name, Config.Instance.FilterInfo.TargetedMobs,RegexOptions.IgnoreCase))
                {
                    // Kill the creature if it has aggroed and aggro is checked. 
                    if (mob.HasAggroed && Config.Instance.FilterInfo.AggroFilter) return true;

                    // Kill the creature if it is claimed by party and party is checked. 
                    if (mob.PartyClaim && Config.Instance.FilterInfo.PartyFilter) return true;

                    // Kill the creature if it's not claimed and unclaimed is checked. 
                    if (!mob.IsClaimed && Config.Instance.FilterInfo.UnclaimedFilter) return true;

                    // Kill the creature if it's claimed and we we don't have claim but
                    // claim is checked. 
                    //FIX: Temporary fix until player.serverid is fixed. 
                    if (mob.IsClaimed && mob.ClaimedID != fface.PartyMember[0].ServerID)
                    {
                        // Kill creature if claim is checked. 
                        if (Config.Instance.FilterInfo.ClaimedFilter) return true;
                    }

                    return false;
                }

                // True for all mobs that are not on ignore / target lists
                // and meet the general criteria for being a valid mob. 
                return true;
            });
        }

        /// <summary>
        /// Check multiple patterns for a match.
        /// </summary>
        /// <param name="input"></param>
        /// <param name="patterns"></param>
        /// <returns></returns>
        private static bool MatchAny(string input, IList<String> patterns, RegexOptions options)
        {
            return patterns
                .Select(pattern => new Regex(pattern, options))
                .Any(matcher => matcher.IsMatch(input));
        }
        #endregion

        public static Func<Unit, bool> PCFilter(FFACE fface)
        {
            // Function to use to filter surrounding mobs by.
            return new Func<Unit, bool>((Unit x) =>
            {
                // No fface? Bail. 
                if (fface == null) return false;

                // PC is null, bail
                // Null check must be kept or null occurs will be thrown
                // secretly by the FSM. 
                if (x == null) return false;

                // PC is not active but in memory
                if (!x.IsActive) return false;

                // Type is not mob 
                if (!x.NPCType.Equals(NPCType.PC)) return false;

                // PC is out of range
                if (x.Distance >= 50) return false;

                // PC can see us...
                return true;
            });
        }
    }
}
