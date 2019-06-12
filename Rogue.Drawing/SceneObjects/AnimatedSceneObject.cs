﻿namespace Rogue.Drawing.SceneObjects
{
    using Rogue.Drawing.SceneObjects.Map;
    using Rogue.Entites.Animations;
    using Rogue.Types;
    using Rogue.View.Interfaces;
    using System;
    using System.Collections.Generic;

    public abstract class AnimatedSceneObject : TooltipedSceneObject
    {
        public override bool CacheAvailable => false;

        public AnimatedSceneObject(string tooltip, Rectangle defaultFramePosition, Action<List<ISceneObject>> showEffects, Func<int, AnimationMap, bool> requestNextFrame = null) : base(tooltip, showEffects)
        {
            this.FramePosition = defaultFramePosition;
            this.RequestNextFrame = requestNextFrame ?? this.DefaultRequestNextFrame;
        }

        private int FrameCounter = 0;

        public bool FreezeForceAnimation { get; set; }

        public override Rectangle ImageRegion
        {
            get
            {
                if (Global.FreezeWorld!=null && !FreezeForceAnimation)
                    return FramePosition;

                FrameCounter++;

                DrawLoop();
                ChangeAnimationFrame();

                return FramePosition;
            }
        }

        public override string Image
        {
            get
            {
                if (animationMap?.TilesetAnimation ?? true)
                {
                    return base.Image;
                }

                return this.animationMap.FullFrames[frame];
            }
            set
            {
                if (animationMap?.TilesetAnimation ?? false)
                    return;

                base.Image = value;
            }
        }

        private int frame = 0;

        private bool animationStop = false;

        protected bool RequestStop()
        {
            FrameCounter = 0;
            return animationStop = true;
        }

        protected bool RequestResume() => animationStop = false;

        private AnimationMap animationMap;

        protected void SetAnimation(AnimationMap animationMap) => this.animationMap = animationMap;

        protected Rectangle FramePosition;

        protected Func<int, AnimationMap, bool> RequestNextFrame;

        private bool DefaultRequestNextFrame(int frameCounter, AnimationMap animMap)
        {
            var framesPerSec = animMap.FramesPerSecond == default
                ? animMap.Frames.Count
                : animMap.FramesPerSecond;

            return frameCounter % (60 / framesPerSec) == 0;
        }

        protected virtual void ChangeAnimationFrame()
        {
            if (animationMap != null && !animationStop)
            {
                if (animationMap.TilesetAnimation)
                {
                    FramePosition.Pos = animationMap.Frames[frame];
                }

                if (RequestNextFrame(FrameCounter, animationMap))
                {
                    frame++;
                    AnimationLoop();
                }

                var countOfFrames = animationMap.TilesetAnimation
                    ? animationMap.Frames.Count
                    : animationMap.FullFrames.Length;

                if (frame == countOfFrames)
                {
                    frame = 0;
                    FrameCounter = 0;
                }
            }
        }

        protected virtual void AnimationLoop() { }

        protected abstract void DrawLoop();
    }
}