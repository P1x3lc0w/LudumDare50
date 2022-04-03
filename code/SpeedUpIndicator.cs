using Godot;
using System;

public class SpeedUpIndicator : TextureRect
{

    public override void _Process(float delta)
    {
        Visible = Player.Instance.speedUpTime > 0;
    }
}
