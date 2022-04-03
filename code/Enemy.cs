using Godot;
using System;

public class Enemy : KinematicBody2D
{
    protected float visionRange = 98f;
    public float health = 10f;
    protected float normalSpeed = 30f;
    protected float chaseSpeed = 55f;
    protected float movementStunTime = 0f;

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {
        movementStunTime -= delta;
        if (movementStunTime <= 0)
        {
            bool canSeePlayer = Player.Instance.GlobalPosition.DistanceTo(GlobalPosition) <= visionRange;
            Vector2 target = canSeePlayer ? Player.Instance.GlobalPosition : Fire.Instance.GlobalPosition;

            MoveAndSlide((target - GlobalPosition).Normalized() * (canSeePlayer ? chaseSpeed : normalSpeed));

            int collisionCount = GetSlideCount();
            for (int i = 0; i < collisionCount; i++)
            {
                KinematicCollision2D collision = GetSlideCollision(i);
                switch (collision.Collider)
                {
                    case Fire fire:
                        GetParent()?.RemoveChild(this);
                        if (Player.Instance.shieldTime > 0)
                            break;
                        fire.FireTime -= 10;
                        break;

                    case Player player:
                        movementStunTime = 2f;
                        if (Player.Instance.shieldTime > 0)
                            break;
                        Player.Instance.Hit(MainMenu.difficulty.enemyContactDamage);
                        break;
                }
            }
        }
    }

    public override void _ExitTree()
    {
        GameWorld.Instance?.EnemyRemoved(this);
    }

    public void Hit(float damage)
    {
        health -= damage;
        MainMenu.Instance.enemyHitAudioPlayer.Play();
        if (health < 0)
        {
            GetParent()?.RemoveChild(this);
        }
    }
}
