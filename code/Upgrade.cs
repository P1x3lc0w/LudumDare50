using Godot;
using System;

public class Upgrade : VBoxContainer
{
    public string name;
    public Texture texture;
    public Action selectCallback;
    public override void _Ready()
    {
        GetNode<Label>("Label").Text = name;
        GetNode<TextureRect>("TextureRect").Texture = texture;
    }

    private void _on_Button_pressed() {
        selectCallback?.Invoke();
        RewardUI.Instance.UpgradeSelected();
    }
}
