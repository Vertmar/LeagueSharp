using LeagueSharp;
using LeagueSharp.Common;
using System;
using Color = System.Drawing.Color;

namespace VRgragas
{
    class VRgragas
    {
        public static Menu Menu;
        public static Orbwalking.Orbwalker Orbwalker;
        public static Spell Q, W, E, R;
        public static bool PacketCast = true;
        public static void OnGameLoad(EventArgs args)
        {
            Q = new Spell(SpellSlot.Q, 850);
            W = new Spell(SpellSlot.W, bjectManager.Player.AttackRange);
            E = new Spell(SpellSlot.E, 600);
            R = new Spell(SpellSlot.R, 1150);

            Q.SetSkillshot(0.25f, 80f, 1200f, true, SkillshotType.SkillshotCircle);
            W.SetSkillshot(0.25f, 150f, 1200f, false, SkillshotType.SkillshotLine);
            E.SetSkillshot(0.25f, 275f, 1300f, false, SkillshotType.SkillshotLine);
            R.SetSkillshot(1.35f, 190f, float.MaxValue, false, SkillshotType.SkillshotCircle);

            SetupMenu();

            Drawing.OnDraw += OnDraw;

            AntiGapcloser.OnEnemyGapcloser += QGapCloser.OnEnemyGapCloser;

            Orbwalking.AfterAttack += LuxCombo.AfterAttack;
        }

        private static void OnDraw(EventArgs args)
        {
            var drawQ = Menu.Item("drawQ").GetValue<bool>();
            var drawE = Menu.Item("drawE").GetValue<bool>();
            var drawR = Menu.Item("drawR").GetValue<bool>();

            var qColor = Menu.Item("qColor").GetValue<Circle>().Color;
            var eColor = Menu.Item("eColor").GetValue<Circle>().Color;
            var rColor = Menu.Item("rColor").GetValue<Circle>().Color;

            var position = ObjectManager.Player.Position;

            if(drawQ)
                Utility.DrawCircle(position, Q.Range, qColor);          

            if (drawE)
                Utility.DrawCircle(position, E.Range, eColor);

            if (drawR)
                Utility.DrawCircle(position, R.Range, rColor);

        }

        private static void SetupMenu()
        {
            Menu = new Menu("VRgragas", "VRgragas", true);

            // Target Selector
            var tsMenu = new Menu("[VRgragas] - TS", "VRgragas");
            SimpleTs.AddToMenu(tsMenu);
            Menu.AddSubMenu(tsMenu);

            // Orbwalker
            var orbwalkerMenu = new Menu("[VRgragas] - Orbwalker", "VRgragasOrbwalker");
            Orbwalker = new Orbwalking.Orbwalker(orbwalkerMenu);
            Menu.AddSubMenu(orbwalkerMenu);

            // Combo settings
            var comboMenu = new Menu("[VRgragas] - Combo", "VRgragas");
            comboMenu.AddItem(new MenuItem("useQ", "Use Q").SetValue(true));
            comboMenu.AddItem(new MenuItem("useW", "Use W").SetValue(false));
            comboMenu.AddItem(new MenuItem("useE", "Use E").SetValue(true));
            comboMenu.AddItem(new MenuItem("useR", "Use R").SetValue(true));
            comboMenu.AddItem(new MenuItem("onlyRIfKill", "Use R to kill only").SetValue(false));
            Menu.AddSubMenu(comboMenu);

            // Harass Settings
            var harassMenu = new Menu("[VRgragas] - Harass", "VRgragasHarass");
            harassMenu.AddItem(new MenuItem("useQHarass", "Use Q").SetValue(true));
            harassMenu.AddItem(new MenuItem("useEHarass", "Use E").SetValue(true));
            harassMenu.AddItem(new MenuItem("aaHarass", "Auto attack after harass").SetValue(true));
            Menu.AddSubMenu(harassMenu);

            // KS / Finisher Settings
            var ksMenu = new Menu("[VRgragas] - KS", "cVRgragasKS");
            ksMenu.AddItem(new MenuItem("ultKS", "KS with R").SetValue(true));
            //ksMenu.AddItem(new MenuItem("recallExploitKS", "KS enemies recalling").SetValue(true));
            Menu.AddSubMenu(ksMenu);

            // Items
            var itemsMenu = new Menu("[VRgragas] - Items", "VRgragasItems");
            itemsMenu.AddItem(new MenuItem("useDFG", "Use DFG").SetValue(true));
            Menu.AddSubMenu(itemsMenu);

            //Drawing
            var drawingMenu = new Menu("[VRgragas] - Drawing", "VRgragasDrawing");
            drawingMenu.AddItem(new MenuItem("drawQ", "Draw Q").SetValue(true));
            drawingMenu.AddItem(new MenuItem("drawE", "Draw E").SetValue(true));
            drawingMenu.AddItem(new MenuItem("drawR", "Draw R").SetValue(true));
            drawingMenu.AddItem(new MenuItem("qColor", "Q Color").SetValue(new Circle(true, Color.Gray)));
            drawingMenu.AddItem(new MenuItem("eColor", "E Color").SetValue(new Circle(true, Color.Gray)));
            drawingMenu.AddItem(new MenuItem("rColor", "R Color").SetValue(new Circle(true, Color.Gray)));
            Menu.AddSubMenu(drawingMenu);

            // Combo / Harass
            Menu.AddItem(new MenuItem("combo", "Combo!").SetValue(new KeyBind(32, KeyBindType.Press)));
            //Menu.AddItem(new MenuItem("harass", "Harass!").SetValue(new KeyBind('v', KeyBindType.Press)));

            // Finalize
            Menu.AddToMainMenu();
        }
    }
}
