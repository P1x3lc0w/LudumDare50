using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Godot;

public class ShieldIndicator : Control
{
    public override void _Process(float delta)
    {
        Visible = Player.Instance.shieldTime > 0;
    }
}
