using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Godot;

public class ShootEnemy : Enemy
{
    [Export]
    private PackedScene projectileScene;
    private float projectileTimer;
    public override void _Process(float delta)
    {
        float playerDistance = Player.Instance.GlobalPosition.DistanceTo(GlobalPosition);
        bool canSeePlayer = playerDistance <= visionRange;
        movementStunTime -= delta;
        if (movementStunTime <= 0)
        {

            Vector2 target = canSeePlayer ? Player.Instance.GlobalPosition : Fire.Instance.GlobalPosition;

            MoveAndSlide((target - GlobalPosition).Normalized() * (canSeePlayer ? chaseSpeed : normalSpeed));

            if (canSeePlayer && playerDistance < 64f)
            {
                movementStunTime += 1f;
            }

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

        if (canSeePlayer)
        {
            projectileTimer -= delta;
            if (projectileTimer <= 0)
            {
                Vector2 direction = (Player.Instance.GlobalPosition - GlobalPosition).Normalized();

                for (int i = -MainMenu.difficulty.enemyProjectileCount; i <= MainMenu.difficulty.enemyProjectileCount; i++)
                {
                    Vector2 directionRotated = direction.Rotated(0.2f * i);
                    EnemyProjectile enemyProjectile = projectileScene.Instance<EnemyProjectile>();
                    enemyProjectile.Position = GlobalPosition + (directionRotated * 16f);
                    enemyProjectile.velocity = directionRotated * MainMenu.difficulty.enemyProjectileSpeed;
                    GetParent().AddChild(enemyProjectile);
                }
                MainMenu.Instance.enemyShootAudioPlayer.Play();
                projectileTimer = MainMenu.difficulty.enemyProjectileInterval;
            }
        }
    }
}
