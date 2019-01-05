// ///////////////////////////////////////////////////////////////////
// This file is a part of EasyFarm for Final Fantasy XI
// Copyright (C) 2013 Mykezero
//  
// EasyFarm is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//  
// EasyFarm is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// If not, see <http://www.gnu.org/licenses/>.
// ///////////////////////////////////////////////////////////////////
using System.Linq;
using EasyFarm.Classes;
using EasyFarm.Context;
using MemoryAPI;

namespace EasyFarm.States
{
    public class SummonTrustsState : BaseState
    {
        private bool PartyHasSpace(IGameContext context)
        {
            var slots = 0;
            for (var i = 1; i < 6; i++)
                if (!context.API.PartyMember[(byte) i].UnitPresent)
                    slots++;
            return slots > 0;
        }

        private IPartyMemberTools FindPartyMember(IGameContext context, BattleAbility trust)
        {
            if (string.IsNullOrEmpty(trust.Name)) return null;

            for (var i = 1; i < 6; i++)
            {
                var p = context.API.PartyMember[(byte) i];
                var comp = trust.Name;
                if (comp.Contains("(UC)") || comp.Contains("II") || comp.Contains("AA"))
                {
                    comp = comp.Replace(" (UC)", "");
                    comp = comp.Replace(" II", "");
                    comp = comp.Replace("AA", "Ark");
                }

                comp = comp.Replace(" ", "");

                if (p.UnitPresent && p.Name == comp) return p;
            }

            return null;
        }

        private bool TrustInParty(IGameContext context, BattleAbility trust)
        {
            var t = FindPartyMember(context, trust);
            return t != null;
        }

        private bool TrustNeedsSummoning(IGameContext context, BattleAbility trust)
        {
            if (TrustInParty(context, trust) && TrustNeedsDismissal(context, trust)) ReleaseTrust(context, trust);
            return !TrustInParty(context, trust) && PartyHasSpace(context);
        }

        private bool TrustNeedsDismissal(IGameContext context, BattleAbility trust)
        {
            var t = FindPartyMember(context, trust);
            if (t == null) return false;

            // If the trust is set to be resummonable, respect the MP.
            if (trust.ResummonOnLowMP)
                if (t.MPPCurrent <= trust.ResummonMPHigh && t.MPPCurrent >= trust.ResummonMPLow)
                    return true;

            // If the trust is set to be resummonable, respect the HP 
            if (trust.ResummonOnLowHP)
                if (t.HPPCurrent <= trust.ResummonHPHigh && t.HPPCurrent >= trust.ResummonHPLow)
                    return true;

            return false;
        }

        private void ReleaseTrust(IGameContext context, BattleAbility trust)
        {
            var comp = trust.Name;
            if (comp.Contains("(UC)") || comp.Contains("II") || comp.Contains("AA"))
            {
                comp = comp.Replace(" (UC)", "");
                comp = comp.Replace(" II", "");
                comp = comp.Replace("AA", "Ark");
            }

            comp = comp.Replace(" ", "");

            var command = string.Format("/refa {0}", comp);
            context.API.Windower.SendString(command);
        }

        public override bool Check(IGameContext context)
        {
            if (new RestState().Check(context)) return false;
            if (!context.API.Player.Status.Equals(Status.Standing)) return false;

            var trusts = context.Config.BattleLists["Trusts"].Actions
                .Where(t => t.IsEnabled)
                .Where(t => ActionFilters.BuffingFilter(context.API, t))
                .ToList();

            var maxTrustPartySize = context.Config.TrustPartySize;

            foreach (var trust in trusts)
                if (TrustNeedsDismissal(context, trust) ||
                    !TrustInParty(context, trust) && PartyHasSpace(context) && !MaxTrustsReached(context, maxTrustPartySize))
                    return true;

            return false;
        }

        private bool MaxTrustsReached(IGameContext context, int maxTrustPartySize)
        {
            return context.API.PartyMember.Values
                       .Where(x => x.UnitPresent)
                       .Count(x => x.NpcType == NpcType.NPC) >= maxTrustPartySize;
        }

        public override void Run(IGameContext context)
        {
            if (context.API.Player.Status.Equals(Status.Fighting)) return;
            var trusts = context.Config.BattleLists["Trusts"].Actions.Where(t => t.IsEnabled);
            foreach (var trust in trusts)
                if (TrustNeedsSummoning(context, trust) && AbilityUtils.IsRecastable(context.API, trust))
                    context.Memory.Executor.UseActions(new[] {trust});
        }
    }
}