using Godot;
using System;

public class FireTimeDisplay : Label
{
    public override void _Ready()
    {

    }

    private Color normalColor = new Color(1, 1, 1, 1);
    private Color lowTimeColor = new Color(1, 0.25f, 0.25f, 1);
    public override void _Process(float delta)
    {
        Text = Fire.Instance.FireTime.ToString("#");
        SelfModulate = Fire.Instance.FireTime > 20f ? normalColor : lowTimeColor;
    }
}
