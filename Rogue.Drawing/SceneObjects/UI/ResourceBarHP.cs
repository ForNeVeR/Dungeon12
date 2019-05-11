﻿namespace Rogue.Drawing.SceneObjects.UI
{
    using Rogue.Drawing.Impl;
    using Rogue.Entites.Alive.Character;
    using Rogue.View.Interfaces;
    using System;

    public class ResourceBarHP : ResourceBar<Player>
    {
        private readonly ImageControl HpBar;

        public ResourceBarHP(Player avatar) : base(avatar)
        {
            HpBar = new InteractiveHPBar(avatar)
            {
                Left = 0.031,
                Top = 0.031,
                Height = 0.46875
            };

            this.AddChild(HpBar);
        }

        protected override string BarTile => "Rogue.Resources.Images.ui.player.hp_back.png";


        private class InteractiveHPBar : ImageControl
        {
            private Player player;

            private IDrawText hpText;

            public InteractiveHPBar(Player player) : base("Rogue.Resources.Images.ui.player.hp.png")
            {
                this.player = player;

                hpText = new DrawText($"{player.HitPoints}/{player.MaxHitPoints}", ConsoleColor.White)
                {
                    Size = 14
                }.Monserrat();

                var text = this.AddTextCenter(hpText);
                    text.Top += 0.2;
                text.Left += 0.2;
            }

            public override double Width
            {
                get
                {
                    hpText.SetText($"{player.HitPoints}/{player.MaxHitPoints}");
                    return 4.75 * (((double)player.HitPoints / player.MaxHitPoints * 100) / 100);
                }
                set { }
            }

            public override bool CacheAvailable => false;
        }

    }
}