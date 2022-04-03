using Godot;

public class WoodCounter : Label
{
    public override void _Process(float delta)
        => Text = Player.Instance.woodCount.ToString();
}
