using Godot;
using System;

public class Tree : StaticBody2D, IInteractable
{
    [Export]
    private NodePath interactionDisplayPath;
    private Label interactionDisplay;
    private float interactTimer = 3f;
    public override void _Ready()
    {
        interactionDisplay = GetNode<Label>(interactionDisplayPath);
    }

    public void Focus()
    {
        interactionDisplay.Visible = true;
    }

    public void Blur()
    {
        interactionDisplay.Visible = false;
    }
    public bool BeginInteract()
    {
        interactTimer = MainMenu.difficulty.treeInteractTime;
        return true;
    }

    public bool Update(float delta)
    {
        interactTimer -= delta;
        interactionDisplay.Text = interactTimer.ToString("0.0");
        if(interactTimer <= 0) {
            Player.Instance.woodCount++;
            GetParent().RemoveChild(this);
            return false;
        } else return true;
    }
}
