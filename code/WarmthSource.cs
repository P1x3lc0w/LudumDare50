using Godot;
using System;
using System.Collections.Generic;

public class WarmthSource : Node2D
{
    [Export]
    public float warmth;
    [Export]
    public float range;
    public static HashSet<WarmthSource> WarmthSources { get; private set;} = new HashSet<WarmthSource>();
    public override void _EnterTree()
    {
        WarmthSources.Add(this);
    }

    public override void _ExitTree()
    {
        WarmthSources.Remove(this);
    }
}
