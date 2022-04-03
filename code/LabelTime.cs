using Godot;
using System;

public class LabelTime : Label
{
    public override void _Process(float delta)
    {
        Text = $"{Mathf.Floor(GameWorld.Instance.GameTime/60):00}:{GameWorld.Instance.GameTime%60:00}";
    }
}
