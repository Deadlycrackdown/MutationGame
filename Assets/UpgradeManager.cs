using UnityEngine;
using UnityEngine.UI;

public class UpgradeManager : MonoBehaviour
{
    [SerializeField]
    private MutationNest mutationNest;

    public int MutationPoints
    {
        get { return mutationNest.mutationPoints; }
        set { mutationNest.mutationPoints = value; }
    }

    public UpgradeButton[] upgradeButtons;

    private void Start()
    {
        UpdateButtons();
    }

    public void SelectUpgrade(UpgradeType upgradeType)
    {
        int upgradeCost = GetUpgradeCost(upgradeType);
        if (MutationPoints >= upgradeCost)
        {
            MutationPoints -= upgradeCost;
            ApplyUpgrade(upgradeType);
            UpdateButtons();
        }
    }

    public int GetUpgradeCost(UpgradeType upgradeType)
    {
        switch (upgradeType)
        {
            case UpgradeType.SimultaneousTentacles:
                return 5; // Example upgrade cost, adjust as needed
            case UpgradeType.EnemyPullTime:
                return 10; // Example upgrade cost, adjust as needed
            case UpgradeType.TentacleRange:
                return 8; // Example upgrade cost, adjust as needed
            case UpgradeType.SpecialAbilities:
                return 15; // Example upgrade cost, adjust as needed
            default:
                return 0; // Default case, no cost
        }
    }

    public bool CanAffordUpgrade(UpgradeType upgradeType)
    {
        int upgradeCost = GetUpgradeCost(upgradeType);
        return MutationPoints >= upgradeCost;
    }

    private void ApplyUpgrade(UpgradeType upgradeType)
    {
        switch (upgradeType)
        {
            case UpgradeType.SimultaneousTentacles:
                mutationNest.UpgradeSimultaneousTentacles(mutationNest.simultaneousTentacles + 1);
                break;
            case UpgradeType.EnemyPullTime:
                mutationNest.UpgradeEnemyPullTime(mutationNest.enemyPullTime * 0.8f); // Reduce pull time by 20%
                break;
            case UpgradeType.TentacleRange:
                mutationNest.UpgradeTentacleRange(mutationNest.tentacleRange + 2f); // Increase range by 2 units
                break;
            case UpgradeType.SpecialAbilities:
                mutationNest.UpgradeSpecialAbilities(true);
                break;
        }
    }

    private void UpdateButtons()
    {
        foreach (UpgradeButton upgradeButton in upgradeButtons)
        {
            upgradeButton.UpdateButton();
        }
    }

#if UNITY_EDITOR
    private void OnValidate()
    {
        if (mutationNest != null)
        {
            MutationPoints = mutationNest.mutationPoints;
        }
    }
#endif
}
