﻿namespace Rogue.Drawing.SceneObjects
{
    using Rogue.Drawing.Impl;
    using Rogue.Settings;
    using Rogue.Types;
    using Rogue.View.Interfaces;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public abstract class SceneObject : ISceneObject
    {
        protected TextControl AddTextCenter(IDrawText drawText, bool horizontal = true, bool vertical=true)
        {
            var textControl = new TextControl(drawText);

            var measure = Global.DrawClient.MeasureText(textControl.Text);

            var width = this.Width * 32;
            var height = this.Height * 32;

            if (horizontal)
            {
                var left = width / 2 - measure.X / 2;
                textControl.Left = left / 32;
            }

            if (vertical)
            {
                var top = height / 2 - measure.Y / 2;
                textControl.Top = top / 32;
            }

            this.Children.Add(textControl);

            return textControl;
        }

        protected Point MeasureText(IDrawText text) => Global.DrawClient.MeasureText(text);

        /// <summary>
        /// Relative
        /// </summary>
        public virtual double Left { get; set; }

        /// <summary>
        /// Relative
        /// </summary>
        public virtual double Top { get; set; }

        /// <summary>
        /// Relative
        /// </summary>
        public virtual double Width { get; set; }

        /// <summary>
        /// Relative
        /// </summary>
        public virtual double Height { get; set; }

        private Rectangle pos = null;

        /// <summary>
        /// Relative position
        /// </summary>
        public virtual Rectangle Position
        {
            get
            {
                if (pos == null || !CacheAvailable)
                {
                    pos =new Rectangle()
                    {
                        X = (float)Left,
                        Y = (float)Top,
                        Width = (float)Width,
                        Height = (float)Height
                    };
                }

                return pos;
            }
        }

        public string Uid { get; } = Guid.NewGuid().ToString();

        public virtual string Image { get; set; }

        public virtual Rectangle ImageRegion { get; set; }

        public virtual IDrawText Text { get; protected set; }

        public virtual IDrawablePath Path { get; }
        
        public ICollection<ISceneObject> Children { get; } = new List<ISceneObject>();

        protected void AddChild(ISceneObject sceneObject)
        {
            if(sceneObject is SceneObject sceneControlObject)
            {
                sceneControlObject.Parent = this;
            }

            this.Children.Add(sceneObject);
        }

        protected void RemoveChild(ISceneObject sceneObject)
        {
            this.Children.Remove(sceneObject);
        }
        
        private Rectangle _computedPosition;
        public Rectangle ComputedPosition
        {
            get
            {
                if (_computedPosition == null || !CacheAvailable)
                {
                    var parentX = Parent?.ComputedPosition?.X ?? 0f;
                    var parentY = Parent?.ComputedPosition?.Y ?? 0f;

                    _computedPosition = new Rectangle
                    {
                        X = parentX + (float)Left,
                        Y = parentY + (float)Top
                    };
                }

                return _computedPosition;
            }
        }

        public ISceneObject Parent { get; set; }

        public virtual bool CacheAvailable => true;

        public virtual bool IsBatch => false;

        public virtual bool Expired => false;

        public virtual bool AbsolutePosition => false;

        public Action Destroy { get; set; }

        public Action<List<ISceneObject>> ShowEffects { get; set; }

        public virtual Rectangle CropPosition => new Rectangle
        {
            X = this.Position.X,
            Y = this.Position.Y,
            Height=this.Children.Max(c=>c.Position.Y+c.Position.Height),
            Width = this.Children.Max(c => c.Position.X + c.Position.Width)
        };

        public int Layer { get; set; }
    }
}
