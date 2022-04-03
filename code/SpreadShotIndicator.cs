using Godot;
using System;

public class SpreadShotIndicator : TextureRect
{
    public override void _Process(float delta)
    {
        Visible = Player.Instance.spreadShotUnlocked && Player.Instance.spreadShotCooldown <= 0;
    }
}
