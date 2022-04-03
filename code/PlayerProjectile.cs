using Godot;
using System;

public class PlayerProjectile : KinematicBody2D
{
    [Export]
    private int health;
    [Export]
    private int damage;
    public Vector2 velocity;

    public override void _Process(float delta)
    {
        MoveAndSlide(velocity);

        int collisionCount = GetSlideCount();
        for (int i = 0; i < collisionCount; i++)
        {
            KinematicCollision2D collision = GetSlideCollision(i);
            switch (collision.Collider)
            {
                case Enemy enemy:
                    enemy.Hit(Player.Instance.damageUpTime > 0 ? 10f * damage : damage);
                    health--;
                    break;

                case EnemyProjectile enemyProjectile:
                    enemyProjectile.GetParent()?.RemoveChild(enemyProjectile);
                    health--;
                    break;

                default:
                    health = 0;
                    break;
            }
        }

        if (health <= 0)
        {
            GetParent()?.RemoveChild(this);
        }
    }
}
