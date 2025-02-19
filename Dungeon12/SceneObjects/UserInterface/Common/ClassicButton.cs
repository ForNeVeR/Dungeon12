﻿using Dungeon;
using Dungeon.Control;
using Dungeon.Drawing;
using Dungeon.SceneObjects;
using System;

namespace Dungeon12.SceneObjects.UserInterface.Common
{
    internal class ClassicButton : EmptySceneControl
    {
        public override void Throw(Exception ex)
        {
            throw ex;
        }

        readonly TextObject Label;

        public ClassicButton(string text)
        {
            this.Width = 250;
            this.Height = 65;

            this.AddBorder(0);

            this.Label= this.AddTextCenter(text.Gabriela().InColor(Global.CommonColorLight).InSize(24));
            this.Image="UI/btn_a.png";
        }

        private bool _disabled;
        public bool Disabled
        {
            get => _disabled;
            set
            {
                _disabled = value;
                if (value)
                    Label.Text.ForegroundColor = DrawColor.Gray;
                else
                    Label.Text.ForegroundColor = Global.CommonColorLight;
            }
        }

        public void SetText(string txt)
        {
            Label.Text.SetText(txt);
        }

        public Action OnClick { get; set; }

        public override void Click(PointerArgs args)
        {
            OnClick?.Invoke();
        }

        public override void Focus()
        {
            if (Disabled)
                return;

            DungeonGlobal.AudioPlayer.Effect("focus.wav".AsmSoundRes());
            this.Image="UI/btn_b.png";
        }

        public override void Unfocus()
        {
            if (Disabled)
                return;
            this.Image="UI/btn_a.png";
        }
    }
}
