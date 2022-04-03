using Godot;
using System;

public class MainMenu : CenterContainer
{
    public static MainMenu Instance;
    public static GameDifficulty difficulty;
    [Export]
    private NodePath menuPath;
    private Control menu;
    [Export]
    private NodePath gameOverLabelPath;
    private Label gameOverLabel;
    [Export]
    private PackedScene gameScene;
    public GameWorld currentGame;
    //Audio Players
    [Export]
    private NodePath enemyHitAudioPlayerPath;
    public AudioStreamPlayer enemyHitAudioPlayer;
    [Export]
    private NodePath enemyShootAudioPlayerPath;
    public AudioStreamPlayer enemyShootAudioPlayer;
    [Export]
    private NodePath fireDownAudioPlayerPath;
    public AudioStreamPlayer fireDownAudioPlayer;
    [Export]
    private NodePath fireUpAudioPlayerPath;
    public AudioStreamPlayer fireUpAudioPlayer;
    [Export]
    private NodePath playerHitAudioPlayerPath;
    public AudioStreamPlayer playerHitAudioPlayer;
    [Export]
    private NodePath playerShootAudioPlayerPath;
    public AudioStreamPlayer playerShootAudioPlayer;
    public override void _Ready()
    {
        menu = GetNode<Control>(menuPath);
        gameOverLabel = GetNode<Label>(gameOverLabelPath);

        enemyHitAudioPlayer = GetNode<AudioStreamPlayer>(enemyHitAudioPlayerPath);
        enemyShootAudioPlayer = GetNode<AudioStreamPlayer>(enemyShootAudioPlayerPath);
        fireDownAudioPlayer = GetNode<AudioStreamPlayer>(fireDownAudioPlayerPath);
        fireUpAudioPlayer = GetNode<AudioStreamPlayer>(fireUpAudioPlayerPath);
        playerHitAudioPlayer = GetNode<AudioStreamPlayer>(playerHitAudioPlayerPath);
        playerShootAudioPlayer = GetNode<AudioStreamPlayer>(playerShootAudioPlayerPath);

        Instance = this;
    }
    public void GameOver(string reason)
    {
        gameOverLabel.Text = $"Game Over: {reason}.\nYou survived for {Mathf.Floor(GameWorld.Instance.GameTime/60):00}:{GameWorld.Instance.GameTime%60:00} and completed {GameWorld.Instance.CurrentWave - 1} waves.";
        CloseGame();
    }
    public void CloseGame()
    {
        if (currentGame != null)
        {
            RemoveChild(currentGame);
            currentGame = null;
        }

        menu.Visible = true;
    }
    public void StartGame()
    {
        menu.Visible = false;
        if (currentGame != null)
            RemoveChild(currentGame);

        currentGame = gameScene.Instance<GameWorld>();

        AddChild(currentGame);
    }
    private void _on_ButtonEasy_pressed()
    {
        difficulty = new GameDifficulty()
        {
            enemyContactDamage = 3f,
            enemyProjectileDamage = 1f,
            enemySpawnTime = 15f,
            enemyProjectileCount = 1,
            enemyProjectileSpeed = 80f,
            enemyProjectileInterval = 3f,
            treeInteractTime = 1f,
            enemiesPerWaveMin = 3,
            enemiesPerWaveMax = 5,
            rewardCount = 3,
            warmthChange = .5f
        };
        StartGame();
    }

    private void _on_ButtonNormal_pressed()
    {
        difficulty = new GameDifficulty()
        {
            enemyContactDamage = 5f,
            enemyProjectileDamage = 2f,
            enemySpawnTime = 7f,
            enemyProjectileCount = 2,
            enemyProjectileSpeed = 90f,
            enemyProjectileInterval = 1.5f,
            treeInteractTime = 2f,
            enemiesPerWaveMin = 4,
            enemiesPerWaveMax = 7,
            rewardCount = 2,
            warmthChange = 1.5f,
        };
        StartGame();
    }

    private void _on_ButtonHard_pressed()
    {
        difficulty = new GameDifficulty()
        {
            enemyContactDamage = 7f,
            enemyProjectileDamage = 3f,
            enemySpawnTime = 5f,
            enemyProjectileCount = 3,
            enemyProjectileSpeed = 100f,
            enemyProjectileInterval = 1f,
            treeInteractTime = 2f,
            enemiesPerWaveMin = 5,
            enemiesPerWaveMax = 10,
            rewardCount = 2,
            warmthChange = 3f
        };
        StartGame();
    }

    private void _on_ButtonExit_pressed()
    {
        GetTree().Quit();
    }
}
