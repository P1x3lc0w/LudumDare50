using Godot;
using System;

public class Player : KinematicBody2D
{
    public static Player Instance;
    [Export]
    private PackedScene projectileScene;
    [Export]
    private PackedScene flareScene;
    [Export]
    private NodePath spriteNorthPath;
    private Sprite spriteNorth;
    [Export]
    private NodePath spriteSouthPath;
    private Sprite spriteSouth;
    [Export]
    private NodePath spriteEastPath;
    private Sprite spriteEast;
    [Export]
    private NodePath spriteWestPath;
    private Sprite spriteWest;
    private Vector2 velocity;
    private float speed = 50f;
    private float acceleration = 10f;
    private float warmthThreshold = 20f;
    public float warmth = 20f;
    public float maxWarmth = 20f;
    public float health = 20f;
    public float maxHealth = 20f;
    public int woodCount;
    private float interactionRange = 32f;
    private IInteractable currentInteractable;
    bool interacting;
    private float shootCooldown = 0f;
    public float shieldTime;
    public float speedUpTime;
    public float damageUpTime;
    public bool spreadShotUnlocked;
    public float spreadShotCooldown;
    public bool flareShotUnlocked;
    public float flareCooldown;
    public override void _Ready()
    {
        Instance = this;
        spriteNorth = GetNode<Sprite>(spriteNorthPath);
        spriteSouth = GetNode<Sprite>(spriteSouthPath);
        spriteEast = GetNode<Sprite>(spriteEastPath);
        spriteWest = GetNode<Sprite>(spriteWestPath);
    }

    public override void _Process(float delta)
    {
        if (health < 0)
        {
            MainMenu.Instance.GameOver("You died");
            return;
        }

        shootCooldown -= delta;
        spreadShotCooldown -= delta;
        flareCooldown -= delta;

        shieldTime -= delta;
        speedUpTime -= delta;
        damageUpTime -= delta;

        if (interacting)
        {
            interacting = currentInteractable.Update(delta);
        }
        else
        {

            Vector2 move_dir = Vector2.Zero;

            if (Input.IsActionPressed("move_north"))
            {
                move_dir += Vector2.Up;
            }
            else if (Input.IsActionPressed("move_south"))
            {
                move_dir += Vector2.Down;
            }
            else if (Input.IsActionPressed("move_east"))
            {
                move_dir += Vector2.Right;
            }
            else if (Input.IsActionPressed("move_west"))
            {
                move_dir += Vector2.Left;
            }

            UpdateSprite(move_dir);

            velocity = velocity.LinearInterpolate(move_dir * (speed * (speedUpTime > 0 ? 4 : 1)), acceleration * delta);

            velocity = MoveAndSlide(velocity);

            //Interact
            float closestDistance = interactionRange;
            IInteractable closestInteractable = null;

            foreach (IInteractable interactable in GetTree().GetNodesInGroup("interactable"))
            {
                float distance = GlobalPosition.DistanceTo(interactable.GlobalPosition);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestInteractable = interactable;
                }
            }

            if (closestInteractable != currentInteractable)
            {
                if (currentInteractable != null)
                {
                    currentInteractable.Blur();
                }

                if (closestInteractable != null)
                {
                    closestInteractable.Focus();
                }

                currentInteractable = closestInteractable;
            }

            if (currentInteractable != null && Input.IsActionJustPressed("interact"))
            {
                interacting = currentInteractable.BeginInteract();
            }

            if (Input.IsActionJustPressed("shoot") && shootCooldown <= 0)
            {
                Vector2 direction = GetLocalMousePosition().Normalized();
                PlayerProjectile playerProjectile = projectileScene.Instance<PlayerProjectile>();
                playerProjectile.Position = GlobalPosition + (direction * 16f);
                playerProjectile.velocity = direction * 80f;
                GetParent().AddChild(playerProjectile);
                shootCooldown = .5f;
                MainMenu.Instance.playerShootAudioPlayer.Play();
            }


            if (spreadShotUnlocked && Input.IsActionJustPressed("shoot_spread") && spreadShotCooldown <= 0)
            {
                Vector2 direction = GetLocalMousePosition().Normalized();
                for (int i = -2; i < 3; i++)
                {
                    Vector2 directionRotated = direction.Rotated(.1f * i);
                    PlayerProjectile playerProjectile = projectileScene.Instance<PlayerProjectile>();
                    playerProjectile.Position = GlobalPosition + (directionRotated * 16f);
                    playerProjectile.velocity = directionRotated * 80f;
                    GetParent().AddChild(playerProjectile);
                }

                spreadShotCooldown = 1f;
                MainMenu.Instance.playerShootAudioPlayer.Play();
            }

            if (flareShotUnlocked &&  Input.IsActionJustPressed("shoot_flare") && flareCooldown <= 0)
            {
                Vector2 direction = GetLocalMousePosition().Normalized();
                PlayerProjectile playerProjectile = flareScene.Instance<PlayerProjectile>();
                playerProjectile.Position = GlobalPosition + (direction * 16f);
                playerProjectile.velocity = direction * 60f;
                GetParent().AddChild(playerProjectile);
                flareCooldown = 5f;
                MainMenu.Instance.playerShootAudioPlayer.Play();
            }
        }

        //Calculate Warmth
        float totalWarmth = 0f;

        foreach (WarmthSource ws in WarmthSource.WarmthSources)
        {
            if (!ws.IsVisibleInTree())
                continue;

            totalWarmth += Mathf.Lerp(ws.warmth, 0, GlobalPosition.DistanceTo(ws.GlobalPosition) / ws.range);
        }

        warmth += Mathf.Clamp((totalWarmth - warmthThreshold) * .025f, -MainMenu.difficulty.warmthChange, MainMenu.difficulty.warmthChange) * delta;

        warmth = Mathf.Clamp(warmth, 0f, maxWarmth);

        if (warmth <= 0)
        {
            health -= 2f * delta;
        }

    }

    private void UpdateSprite(Vector2 direction)
    {
        if (direction.y < 0)
        {
            if (direction.x < 0 && direction.x < direction.y)
            {
                //West
                spriteNorth.Visible = false;
                spriteSouth.Visible = false;
                spriteEast.Visible = false;
                spriteWest.Visible = true;
            }
            else if (direction.x > 0 && -direction.x < direction.y)
            {
                //East
                spriteNorth.Visible = false;
                spriteSouth.Visible = false;
                spriteEast.Visible = true;
                spriteWest.Visible = false;
            }
            else
            {
                //North
                spriteNorth.Visible = true;
                spriteSouth.Visible = false;
                spriteEast.Visible = false;
                spriteWest.Visible = false;
            }
        }
        else if (direction.y > 0)
        {
            if (direction.x < 0 && direction.x < direction.y)
            {
                //West
                spriteNorth.Visible = false;
                spriteSouth.Visible = false;
                spriteEast.Visible = false;
                spriteWest.Visible = true;
            }
            else if (direction.x > 0 && -direction.x < direction.y)
            {
                //East
                spriteNorth.Visible = false;
                spriteSouth.Visible = false;
                spriteEast.Visible = true;
                spriteWest.Visible = false;
            }
            else
            {
                //South
                spriteNorth.Visible = false;
                spriteSouth.Visible = true;
                spriteEast.Visible = false;
                spriteWest.Visible = false;
            }
        }
        else
        {
            if (direction.x < 0)
            {
                //West
                spriteNorth.Visible = false;
                spriteSouth.Visible = false;
                spriteEast.Visible = false;
                spriteWest.Visible = true;
            }
            else if (direction.x > 0)
            {
                //East
                spriteNorth.Visible = false;
                spriteSouth.Visible = false;
                spriteEast.Visible = true;
                spriteWest.Visible = false;
            }
        }
    }

    public void Hit(float damage)
    {
        health -= damage;
        MainMenu.Instance.playerHitAudioPlayer.Play();
    }
}
