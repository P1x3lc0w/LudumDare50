using Godot;
using System;

public class Fire : StaticBody2D, IInteractable
{
    public static Fire Instance { get; private set; }
    [Export]
    private NodePath interactionDisplayPath;
    private CanvasItem interactionDisplay;
    private float _fireTime = 60.0f;
    public float FireTime
    {
        get => _fireTime;
        set
        {
            if (value > _fireTime)
            {
                MainMenu.Instance?.fireUpAudioPlayer?.Play();
            }
            else if (value < _fireTime)
            {
                MainMenu.Instance?.fireDownAudioPlayer?.Play();
            }
            _fireTime = value;
        }
    }


    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        Instance = this;
        interactionDisplay = GetNode<CanvasItem>(interactionDisplayPath);
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {
        _fireTime -= delta;
        if (FireTime < 0)
        {
            MainMenu.Instance.GameOver("The Fire went out");
        }
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
        if (Player.Instance.woodCount > 0)
        {
            Player.Instance.woodCount--;
            FireTime += 15;
        }
        return false;
    }

    public bool Update(float delta)
    {
        return false;
    }
}
