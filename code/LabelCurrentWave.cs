using Godot;
using System;

public class LabelCurrentWave : Label
{
    public override void _Process(float delta)
    {
        Text = $"Wave {GameWorld.Instance.CurrentWave.ToString()}";
    }
}
