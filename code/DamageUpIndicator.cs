using Godot;
using System;

public class DamageUpIndicator : TextureRect
{
    public override void _Process(float delta)
    {
        Visible = Player.Instance.damageUpTime > 0;
    }
}
