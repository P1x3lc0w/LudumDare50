using Godot;
using System;

public class Shield : Sprite
{
	public override void _Process(float delta)
	{
		Visible = Player.Instance.shieldTime > 0;
	}
}
