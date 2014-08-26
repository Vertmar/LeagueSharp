using System.Linq;
using LeagueSharp;
using LeagueSharp.Common;
using System;

namespace VRgragas
{
    internal class GragasCombo
    {
        private static bool _haveToAa;

        public static void OnGameUpdate(EventArgs args)
        {
            VRgragas.PacketCast = VRgragas.Menu.Item("packetCast").GetValue<bool>();

            if (VRgragas.Menu.Item("ultKS").GetValue<bool>())
            {
                KillSecure();
            }

            if (VRgragas.Menu.Item("combo").GetValue<KeyBind>().Active)
            {
                Combo();
            }

            if (VRgragas.Menu.Item("harass").GetValue<KeyBind>().Active)
            {
                Harass();
            }

        }

        internal static void AfterAttack(Obj_AI_Base unit, Obj_AI_Base target)
        {
            _haveToAa = false;
        }


        private static void KillSecure()
        {
            foreach (var hero in ObjectManager.Get<Obj_AI_Hero>().Where(hero => hero.IsValidTarget()))
            {
                if (SharpDX.Vector2.Distance(hero.ServerPosition.To2D(), hero.ServerPosition.To2D()) > VRgragas.R.Range) return;
                if (VRgragas.R.GetDamage(hero) < hero.Health) return;

                VRgragas.R.Cast(hero, VRgragas.PacketCast);
                return;
            }
        }

        private static void Harass()
        {
            var useQ = VRgragas.Menu.Item("useQHarass").GetValue<bool>();
            var useE = VRgragas.Menu.Item("useEHarass").GetValue<bool>();

            var target = SimpleTs.GetTarget(VRgragas.Q.Range, SimpleTs.DamageType.Magical);
            if (!target.IsValid || _haveToAa) return;

            if (VRgragas.Q.IsReady() && useQ && !_haveToAa)
            {
                VRgragas.Q.CastIfHitchanceEquals(target, HitChance.High, VRgragas.PacketCast);
                if (aaAfterSpell)
                {
                    _haveToAa = true;
                    VRgragas.Orbwalker.ForceTarget(target);
                }
            }

            if (!VRgragas.E.IsReady() || !useE || _haveToAa) return;
            VRgragas.E.Cast(target, VRgragas.PacketCast);

            if (!aaAfterSpell) return;
            _haveToAa = true;
            VRgragas.Orbwalker.ForceTarget(target);
        }

        private static void Combo()
        {
            var useQ = VRgragas.Menu.Item("useQ").GetValue<bool>();
            var useW = VRgragas.Menu.Item("useW").GetValue<bool>();
            var useE = VRgragas.Menu.Item("useE").GetValue<bool>();
            var useR = VRgragas.Menu.Item("useR").GetValue<bool>();

            var target = SimpleTs.GetTarget(VRgragas.Q.Range, SimpleTs.DamageType.Magical);
            var aaAfterSpell = VRgragas.Menu.Item("aaAfterSpell").GetValue<bool>();

            var useDfg = VRgragas.Menu.Item("useDFG").GetValue<bool>();

            if (!target.IsValid || _haveToAa) return;

            if (useDfg)
            {
                if (Items.CanUseItem(3128) && Items.HasItem(3128)) Items.UseItem(3128, target);
            }

            if (VRgragas.Q.IsReady() && useQ && !_haveToAa)
            {
                var castedQ = VRgragas.Q.CastIfHitchanceEquals(target, HitChance.High, VRgragas.PacketCast);
                if (castedQ)
                {
                    if (aaAfterSpell)
                    {
                        _haveToAa = true;
                        VRgragas.Orbwalker.ForceTarget(target);
                    }
                }
            }

            if (VRgragas.E.IsReady() && useE && !_haveToAa)
            {
                VRgragas.E.Cast(target, VRgragas.PacketCast);
                if (aaAfterSpell)
                {
                    _haveToAa = true;
                    VRgragas.Orbwalker.ForceTarget(target);
                }
            }

            if (VRgragas.W.IsReady() && useW)
            {
                    VRgragas.W.Cast(Game.CursorPos, VRgragas.PacketCast);
            }

            if (target.IsDead) return;
            if (!VRgragas.R.IsReady() || !useR || _haveToAa) return;

            if (VRgragas.Menu.Item("onlyRIfKill").GetValue<bool>())
            {
                if (VRgragas.R.GetDamage(target) >= target.Health)
                {
                    VRgragas.R.Cast(target, VRgragas.PacketCast);
                }
            }
            else
            {
                VRgragas.R.Cast(target, VRgragas.PacketCast);
            }
        }
    }
}
