using System;
using UnityEngine;

[DefaultExecutionOrder(-1000)]
public class GameManager : MonoBehaviour
{
    
    public static Action OnRoundBegin;
    public static int CurrentDay { get; private set; }
    public static int DefeatedCount { get; private set; }
    
    [SerializeField] private GameObject menu;
    [SerializeField] private FinalGameUI finalScreen;
    [SerializeField] private GameUI gameUI;
    [SerializeField] private Transform spawnedEnemyParent;
    public static GameManager Instance { get; private set; }
    public static bool GameRunning { get; set; }


    private void Start()
    {
        OnRoundBegin = null;
        if (Instance && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
        Enemy.OnDeath += (x) => DefeatedCount++;
        Cursor.lockState = CursorLockMode.Confined;
        PlayerControls.DisableControls();
    }


    public void BeginRound()
    {
        menu.gameObject.SetActive(false);
        OnRoundBegin?.Invoke();
        GameRunning = true;
        PlayerControls.EnableControls();
    }

    private void Save()
    {
        //Save the current Day
        PlayerPrefs.SetInt("CurrentDay", CurrentDay);
        PlayerPrefs.SetInt("DefeatedCount", DefeatedCount);
        PlayerPrefs.Save();
    }

    private void Load()
    {
        CurrentDay = PlayerPrefs.GetInt("CurrentDay");
        DefeatedCount = PlayerPrefs.GetInt("DefeatedCount");
    }


    public void EndDay()
    {
        menu.SetActive(true);
        Save();
        CurrentDay++;
        GameRunning = false;
        PlayerControls.DisableControls();
    }

    public void GameOver()
    {
        print("Game Over!");
        GameRunning = false;
        PlayerControls.DisableControls();
        finalScreen.gameObject.SetActive(true);

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
        
        Save();
        
        finalScreen.gameObject.SetActive(false);
    }
}
