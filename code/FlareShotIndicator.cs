using Godot;
using System;

public class FlareShotIndicator : TextureRect
{
    public override void _Process(float delta)
    {
        Visible = Player.Instance.flareShotUnlocked && Player.Instance.flareCooldown <= 0;
    }
}
