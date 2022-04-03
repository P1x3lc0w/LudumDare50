using Godot;
using System;

public class EnemyProjectile : KinematicBody2D
{
    public Vector2 velocity;
    public override void _Process(float delta)
    {
        velocity = MoveAndSlide(velocity);

        int collisionCount = GetSlideCount();
        for (int i = 0; i < collisionCount; i++)
        {
            KinematicCollision2D collision = GetSlideCollision(i);
            switch (collision.Collider)
            {
                case Player player:
                    if(Player.Instance.shieldTime > 0)
                        break;
                    Player.Instance.Hit(MainMenu.difficulty.enemyProjectileDamage);
                    break;

                case PlayerProjectile playerProjectile:
                    playerProjectile.GetParent()?.RemoveChild(playerProjectile);
                    break;

                case Fire fire:
                    if(Player.Instance.shieldTime > 0)
                        break;
                    fire.FireTime -= 5;
                    break;
            }
        }

        if (collisionCount > 0)
        {
            GetParent()?.RemoveChild(this);
        }
    }
}
