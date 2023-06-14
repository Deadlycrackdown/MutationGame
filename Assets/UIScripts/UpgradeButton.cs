using UnityEngine;
using UnityEngine.UI;

public enum UpgradeType
{
    SimultaneousTentacles,
    EnemyPullTime,
    TentacleRange,
    SpecialAbilities
}

public class UpgradeButton : MonoBehaviour
{
    public UpgradeType upgradeType;
    public Text upgradeNameText;
    public Text upgradeCostText;
    public Button upgradeButton;

    private UpgradeManager upgradeManager;

    private void Start()
    {
        upgradeManager = FindObjectOfType<UpgradeManager>();
        if (upgradeManager != null)
        {
            UpdateButton();
        }
        upgradeButton.onClick.AddListener(OnUpgradeButtonClicked);
    }

    private void OnEnable()
    {
        MutationNest.OnMutationPointsUpdated += UpdateButton;
    }

    private void OnDisable()
    {
        MutationNest.OnMutationPointsUpdated -= UpdateButton;
    }

    private void OnUpgradeButtonClicked()
    {
        if (upgradeManager != null)
        {
            upgradeManager.SelectUpgrade(upgradeType);
        }
    }

    public void UpdateButton()
    {
        if (upgradeManager == null)
        {
            upgradeManager = FindObjectOfType<UpgradeManager>();
            if (upgradeManager == null)
            {
                Debug.LogWarning("UpgradeManager not found. Make sure it is present in the scene.");
                return;
            }
        }

        int upgradeCost = upgradeManager.GetUpgradeCost(upgradeType);
        bool canAffordUpgrade = upgradeManager.CanAffordUpgrade(upgradeType);
        upgradeNameText.text = upgradeType.ToString();
        upgradeCostText.text = "Cost: " + upgradeCost.ToString();
        upgradeButton.interactable = canAffordUpgrade;
    }
}
