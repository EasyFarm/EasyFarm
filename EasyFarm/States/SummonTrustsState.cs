using MemoryAPI;
using System.Linq;
using EasyFarm.Classes;
using System.Collections.Generic;

namespace EasyFarm.States {
    public class SummonTrustsState : CombatState {
        public Executor Executor { get; set; }

        public SummonTrustsState(IMemoryAPI fface) : base(fface) {
            Executor = new Executor(fface);
        }

        private bool PartyHasSpace() {
            var slots = 0;
            for(int i = 1; i < 6; i++) {
                if (!fface.PartyMember[(byte)i].UnitPresent) slots++;
            }
            return slots > 0;
        }

        private IPartyMemberTools FindPartyMember(BattleAbility trust) {
            for (int i = 1; i < 6; i++) {
                var p = fface.PartyMember[(byte)i];
                var comp = trust.Name;
                if (comp.Contains("(UC)") || comp.Contains("II")) {
                    comp = comp.Replace(" (UC)", "");
                    comp = comp.Replace(" II", "");
                }
                if (p.UnitPresent && p.Name == comp) {
                    return p;
                }
            }
            return null;
        }

        private bool IsCasterTrust(IPartyMemberTools trust) {
            if (!trust.UnitPresent) return false;
            return false;

            // @FIXME: Trusts don't have jobs, need another way to tell.
            //return (
            //    trust.Job == Job.WhiteMage ||
            //    trust.Job == Job.RedMage ||
            //    trust.Job == Job.Scholar ||
            //    trust.Job == Job.BlackMage ||
            //    trust.Job == Job.Geomancer ||
            //    trust.Job == Job.Summon ||
            //    trust.Job == Job.BlueMage ||
            //    trust.Job == Job.Paladin
            //);
        }

        private bool TrustInParty(BattleAbility trust) {
            var t = FindPartyMember(trust);
            return (t != null);
        }

        private bool TrustNeedsSummoning(BattleAbility trust) {
            if (TrustInParty(trust) && TrustNeedsDismissal(trust)) {
                ReleaseTrust(trust);
            }
            return (!TrustInParty(trust) && PartyHasSpace());
        }

        private bool TrustNeedsDismissal(BattleAbility trust) {
            var t = FindPartyMember(trust);

            if (t.HPPCurrent < 60 || (IsCasterTrust(t) && t.MPPCurrent < 95))
                return true;

            return false;
        }

        private void ReleaseTrust(BattleAbility trust) {
            var command = string.Format("/refa {0}", trust.Name);
            fface.Windower.SendString(command);
        }

        public override bool Check() {
            if (new RestState(fface).Check()) return false;
            if (fface.Player.Status.Equals(Status.Fighting)) return false;

            var trusts = Config.Instance.BattleLists["Trusts"].Actions;
            
            return true;
        }

        public override void Run() {
            if (fface.Player.Status.Equals(Status.Fighting)) return;

            var trusts = Config.Instance.BattleLists["Trusts"].Actions;

            var toCast = new List<BattleAbility>();
            foreach(var trust in trusts) {
                if (TrustNeedsSummoning(trust)) {
                    // @TODO: Check for trust summoning cooldown?
                    Executor.UseBuffingActions(new[] { trust });
                }
            }
        }
    }
}
