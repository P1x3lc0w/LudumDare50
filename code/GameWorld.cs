using Godot;
using System;
using System.Collections.Generic;

public class GameWorld : Node2D
{
    public static GameWorld Instance;
    [Export]
    private PackedScene[] enemyScenes;
    [Export]
    private PackedScene treeScene;
    [Export]
    private NodePath entityContainerPath;
    private Node entityContainer;
    private float enemySpawnTimer = 5f;
    private RandomNumberGenerator rng = new RandomNumberGenerator();

    public int CurrentWave { get; private set; }
    private HashSet<Enemy> currentWaveEnemies = new HashSet<Enemy>();
    public int CurrentWaveRemainingEnemies => currentWaveEnemies.Count;
    public int CurrentWaveRemainingSpawns { get; private set; }
    public float GameTime { get; private set; }
    public override void _Ready()
    {
        rng.Randomize();
        entityContainer = GetNode<Node>(entityContainerPath);
        Instance = this;

        float distance = 128f;
        for(int i = 0; i < 200; i++) {
            Tree tree = treeScene.Instance<Tree>();
            tree.Position = new Vector2(rng.RandfRange(-1, 1), rng.RandfRange(-1, 1)).Normalized() * distance;
            this.entityContainer.AddChild(tree);
            distance += 5;
        }
    }


    public override void _Process(float delta)
    {
        GameTime += delta;
        if (CurrentWaveRemainingSpawns > 0)
        {
            enemySpawnTimer -= delta;
            if (enemySpawnTimer <= 0)
            {
                Vector2 direction = new Vector2(rng.RandfRange(-1, 1), rng.RandfRange(-1, 1)).Normalized();
                Enemy enemy = enemyScenes[rng.RandiRange(0, enemyScenes.Length - 1)].Instance<Enemy>();
                enemy.GlobalPosition = direction * 1024f;
                entityContainer.AddChild(enemy);
                currentWaveEnemies.Add(enemy);
                enemySpawnTimer += MainMenu.difficulty.enemySpawnTime;
                CurrentWaveRemainingSpawns--;
            }
        }
        else if (currentWaveEnemies.Count == 0)
        {
            //Next Wave
            CurrentWave++;
            CurrentWaveRemainingSpawns =
                MainMenu.difficulty.enemiesPerWaveMin +
                ((int)(((float)MainMenu.difficulty.enemiesPerWaveMax - (float)MainMenu.difficulty.enemiesPerWaveMin) * ((float)CurrentWave / 10f)));

            if (CurrentWave > 1)
            {
                RewardUI.Instance.ShowRewards();
                GetTree().Paused = true;
            }
        }

    }

    public void EnemyRemoved(Enemy enemy)
    {
        currentWaveEnemies.Remove(enemy);
    }
}
