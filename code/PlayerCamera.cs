using Godot;
using System;

public class PlayerCamera : Camera2D
{
    public static PlayerCamera Instance { get; private set; }
    private float speed = 10.0f;
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        Instance = this;
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {
        if (Player.Instance != null)
        {
            GlobalPosition = GlobalPosition.LinearInterpolate(Player.Instance.GlobalPosition, Mathf.Min(speed * delta, 1.0f));
        }
    }

    public void TeleportToPlayer()
    {
        Position = Player.Instance.GlobalPosition;
    }
}
