using System;
using System.Globalization;
using TMPro;
using UnityEngine;

[DefaultExecutionOrder(100)]
public class MainMenuUI : MonoBehaviour
{
    [SerializeField] private UpgradeHelperUI[] upgrades;
    [SerializeField] private TextMeshProUGUI day;
    [SerializeField] private TextMeshProUGUI coins;
    [SerializeField] private TextMeshProUGUI health;
    [SerializeField] private TextMeshProUGUI defeated;

    //In OnEnable as this component is enabled often
    private void OnEnable()
    {
        day.text = "Day: " + (GameManager.CurrentDay + 1);
        defeated.text = GameManager.DefeatedCount.ToString();
        UpdateUpgrades();
    }

    private void Start()
    {
        //This is in start because when loading we need to fix stats after.
        foreach (UpgradeHelperUI upgrade in upgrades)
        {
            upgrade.Initialize(UpdateUpgrades);
        }
        UpdateUpgrades();
    }

    private void UpdateUpgrades()
    {
        //Update all buttons
        foreach (UpgradeHelperUI upgrade in upgrades)
        {
            upgrade.UpdateButton();
        }
        
        //Update the stats
        UpdateStats();
    }

    private void UpdateStats()
    {
        coins.text = GameManager.Coins.ToString(CultureInfo.InvariantCulture);
        health.text = Castle.Instance.CurrentHealth + "/" + Castle.Instance.MaxHealth;
    }
    

    public void BeginPlay()
    {
        GameManager.Instance.BeginRound();
    }

    public void Quit()
    {
        PlayerPrefs.Save();
        Application.Quit();
    }

    public void ResetStats()
    {
        foreach (UpgradeHelperUI upgrade in upgrades)
        {
            upgrade.InitStats(true);
        }
        UpdateStats();
    }
}
