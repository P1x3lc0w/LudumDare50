using Godot;
using System;

public class LabelEnemiesRemaining : Label
{
    public override void _Process(float delta)
    {
        Text = $"{GameWorld.Instance.CurrentWaveRemainingEnemies + GameWorld.Instance.CurrentWaveRemainingSpawns} enemies remain";
    }
}
