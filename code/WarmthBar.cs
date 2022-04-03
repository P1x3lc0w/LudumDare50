using Godot;
using System;

public class WarmthBar : Control
{
    [Export]
    private NodePath progressPath;
    private TextureProgress progress;
    [Export]
    private NodePath labelPath;
    private Label label;
    public override void _Ready()
    {
        progress = GetNode<TextureProgress>(progressPath);
        label = GetNode<Label>(labelPath);
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {
        if (Player.Instance != null)
        {
            progress.Value = (Player.Instance.warmth / Player.Instance.maxWarmth) * 100.0;
            label.Text = Player.Instance.warmth.ToString("0");
        }
    }
}
