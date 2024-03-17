using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[DefaultExecutionOrder(-1000)]
public class GameManager : MonoBehaviour
{
    public static Action OnRoundBegin;
    public static Action OnGameEnd;
    
    //Game
    public static int CurrentDay { get; private set; }
    
    //Stats (Belong to the game)
    public static float Coins { get; private set; }
    public static int DefeatedCount { get; private set; }
    
    
    [SerializeField] private GameObject menu;
    [SerializeField] private GameObject finalScreen;
    [SerializeField] private GameObject gameUI;
    [SerializeField] private Transform spawnedEnemyParent;

    public static Slider healthBar;
    public static GameManager Instance { get; private set; }
    public static bool GameRunning { get; set; }


    private void OnEnable() // Needs to happen early.
    {
        Load();
        healthBar = gameUI.GetComponentInChildren<Slider>();
        
        OnRoundBegin = null;
        if (Instance && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
        Enemy.OnDeath += (x) =>
        {
            Coins += x.EnemyStats.Value * Castle.Instance.CastleValueMultiplier;   
            DefeatedCount++;
        };
        Cursor.lockState = CursorLockMode.Confined;
        
    }


    public void BeginRound()
    {
        print("BeginRonud");
        OnRoundBegin?.Invoke();
        menu.gameObject.SetActive(false);
        GameRunning = true;
        PlayerControls.EnableControls();
        gameUI.SetActive(true);
    }

    private void Save()
    {
        //Save the current Day
        PlayerPrefs.SetInt("CurrentDay", CurrentDay);
        PlayerPrefs.SetInt("DefeatedCount", DefeatedCount);
        PlayerPrefs.SetFloat("Coins", Coins);
        PlayerPrefs.SetFloat("CurrentHealth", Castle.Instance.CurrentHealth);
        PlayerPrefs.Save();
    }

    private void Load()
    {
        CurrentDay = PlayerPrefs.GetInt("CurrentDay");
        DefeatedCount = PlayerPrefs.GetInt("DefeatedCount");
        Coins = PlayerPrefs.GetFloat("Coins");
        //Let this be handled by the upgrading system. Castle.Instance.CurrentHealth = 100;//(PlayerPrefs.GetFloat("CurrentHealth")); 
    }


    public void EndDay()
    {
        CurrentDay++;
        GameRunning = false;
        PlayerControls.DisableControls();
        menu.SetActive(true);
        Save();
    }

    public void GameOver()
    {
        print("Game Over!");
        GameRunning = false;
        PlayerControls.DisableControls();
        finalScreen.SetActive(true);
        gameUI.SetActive(false);
        finalScreen.transform.GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text = "GAME OVER\nDAY: " + CurrentDay;
        

        for (int i = 0; i < spawnedEnemyParent.childCount; i++)
        {
            spawnedEnemyParent.GetChild(i).GetComponent<Enemy>().enabled = false;
        }
    }

    public void ResetGame()
    {
        //Note, by using player prefs like this (incorrectly) it makes "settings" much harder to implement.
        CurrentDay = 0;
        DefeatedCount = 0;
        Coins = 0;
        print("Resetting Game");
        
        OnGameEnd?.Invoke();
        
        Save();
        
        finalScreen.gameObject.SetActive(false);
        gameUI.SetActive(false);
        menu.SetActive(true);
    }


    public static void RemoveCoins(float upgrade)
    {
        Coins -= upgrade;
        PlayerPrefs.SetFloat("Coins", Coins);
    }
}
