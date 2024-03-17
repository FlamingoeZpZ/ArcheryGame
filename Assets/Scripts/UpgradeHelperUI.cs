using System;
using System.Globalization;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class UpgradeHelperUI : MonoBehaviour
{
    //Reusable script for any project!
    [SerializeField] private TextMeshProUGUI upgradeName;
    [SerializeField] private TextMeshProUGUI cost;
    [SerializeField] private Image icon;
    [SerializeField] private Slider levelUnlocks;
    [SerializeField] private Button button;
    //Take a reference of the bound Upgrade Objects
    [SerializeField] private UpgradeSO[] upgrades;
    [SerializeField] private string upgradeSeriesName;
    private int _currentLevel;

    public void InitStats(bool restore = false)
    {
        if(restore) PlayerPrefs.DeleteKey(upgradeSeriesName);
        //When the game starts, we need to replace data.
        _currentLevel = PlayerPrefs.GetInt(upgradeSeriesName);

        //Go backwards
        int found = -1;
        for (int i = _currentLevel; i >= 0; i--)
        {
            if (upgrades[i].MustApply)
            {
                //Apply this ability effect.
                upgrades[i].Action.Invoke();
                found = i;
                break;
            }
        }

        if (found != _currentLevel)
        {
            //Then we should apply stats, and just apply again.
            upgrades[_currentLevel].Action.Invoke();
        }
        levelUnlocks.maxValue = upgrades.Length;
        RectTransform rt = (RectTransform)levelUnlocks.transform;
        
        //levelUnlocks.fillRect.sizeDelta =new Vector2((upgrades.Length-1) * 32, levelUnlocks.fillRect.sizeDelta.y);
        rt.sizeDelta = new Vector2(upgrades.Length * 32, rt.sizeDelta.y);
        UpdateButton();
    }


    //How do we ensure that the player has the correct level when they start.
    public void Initialize(Action exec)
    {
        button.onClick.AddListener(() => {
            UpdateInfo();
            exec.Invoke();
        });
        InitStats();
    }

    public void UpdateButton()
    {
        button.interactable = CanUpgrade();
        UpgradeSO current = upgrades[Mathf.Min(upgrades.Length-1,_currentLevel+1)];
        upgradeName.text = current.name;
        cost.text = current.Cost.ToString(CultureInfo.InvariantCulture);
        icon.sprite = current.Icon;
        levelUnlocks.value = _currentLevel+1;
    }


    private bool CanUpgrade()
    {
        return _currentLevel < upgrades.Length - 1 && upgrades[_currentLevel+1].Cost <= GameManager.Coins;
    }
    

    private void UpdateInfo()
    {
        GameManager.RemoveCoins(upgrades[++_currentLevel].Cost);
        PlayerPrefs.SetInt(upgradeSeriesName, _currentLevel);
        PlayerPrefs.Save();
        
        upgrades[_currentLevel].Action.Invoke();
        UpdateButton();
    }


}
