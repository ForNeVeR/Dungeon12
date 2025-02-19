﻿using Dungeon.View.Interfaces;
using Dungeon12.SceneObjects.Base;

namespace Dungeon12
{
    static internal class NineSliceBorderExtensions
    {
        /// <summary>
        /// border is 5px
        /// </summary>
        /// <param name="sceneObject"></param>
        /// <param name="opacity"></param>
        public static void AddBorder(this ISceneObject sceneObject, double opacity=.9)
        {
            sceneObject.AddChild(new Border(sceneObject.Width, sceneObject.Height,opacity));
        }
    }
}
