using Godot;
using System;
using System.Collections.Generic;

public class RewardUI : CenterContainer
{
    public static RewardUI Instance { get; private set; }
    [Export]
    private PackedScene choiceScene;
    [Export]
    private NodePath choiceContainerPath;
    private Control choiceContainer;
    [Export]
    private Texture txMaxHealth;
    [Export]
    private Texture txMaxWarmth;
    [Export]
    private Texture txHeal;
    [Export]
    private Texture txWood;
    [Export]
    private Texture txFire;
    [Export]
    private Texture txShield;
    [Export]
    private Texture txSpeed;
    [Export]
    private Texture txDamage;
    [Export]
    private Texture txSpread;
    [Export]
    private Texture txFlare;
    private RandomNumberGenerator rng = new RandomNumberGenerator();

    public override void _Ready()
    {
        rng.Randomize();
        choiceContainer = GetNode<Control>(choiceContainerPath);
        Instance = this;
    }

    public void ShowRewards()
    {
        while (choiceContainer.GetChildCount() > 0)
            choiceContainer.RemoveChild(choiceContainer.GetChild(0));

        List<(string, Texture, Action)> upgrades = new List<(string, Texture, Action)>() {
            ("+ 5 max health", txMaxHealth, () => Player.Instance.maxHealth += 5),
            ("+ 5 max warmth", txMaxWarmth, () => Player.Instance.maxWarmth += 5),
            ("heal 20 health", txHeal, () => Player.Instance.health = Math.Min(Player.Instance.maxHealth, Player.Instance.health + 20)),
            ("+ 5 wood", txWood, () => Player.Instance.woodCount += 5),
            ("fire +20s", txFire, () => Fire.Instance.FireTime += 20),
            ("30s shield", txShield, () => Player.Instance.shieldTime = 30),
            ("40s speed up", txSpeed, () => Player.Instance.speedUpTime = 40),
            ("40s damage up", txDamage, () => Player.Instance.damageUpTime = 40),
        };

        if (!Player.Instance.spreadShotUnlocked)
        {
            upgrades.Add(("Unlock spread shot (press [2])", txSpread, () => Player.Instance.spreadShotUnlocked = true));
        }

        if (!Player.Instance.flareShotUnlocked)
        {
            upgrades.Add(("Unlock flare shot (press [3])", txFlare, () => Player.Instance.flareShotUnlocked = true));
        }

        HashSet<int> chosenUpgrades = new HashSet<int>();

        for (int i = 0; i < MainMenu.difficulty.rewardCount; i++)
        {
            int upgradeIndex;
            do
            {
                upgradeIndex = rng.RandiRange(0, upgrades.Count - 1);
            } while (chosenUpgrades.Contains(upgradeIndex));

            chosenUpgrades.Add(upgradeIndex);

            Upgrade upgrade = choiceScene.Instance<Upgrade>();
            upgrade.name = upgrades[upgradeIndex].Item1;
            upgrade.texture = upgrades[upgradeIndex].Item2;
            upgrade.selectCallback = upgrades[upgradeIndex].Item3;
            choiceContainer.AddChild(upgrade);
        }

        Visible = true;
    }

    public void UpgradeSelected()
    {
        Visible = false;
        GetTree().Paused = false;
    }
}
