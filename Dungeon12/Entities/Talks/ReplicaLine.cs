﻿namespace Dungeon12.Entities.Talks
{
    internal class ReplicaLine
    {
        public Replica Replica { get; set; }

        public string Text { get; set; }

        public ReplicaType Type { get; set; }

        public int Weight { get; set; }
    }
}