using UnityEngine;
using Random = UnityEngine.Random;

public class Spawner : MonoBehaviour
{
    [SerializeField] private Transform[] spawnPoints;
    
    [SerializeField] private RoundSO[] rounds;

    private RoundSO _currentRound;
    
    private int _numEnemies;
    
    private float _timeBetweenSpawns;
    private float _currentSpawnPeriod;

    private int[] _enemyCounts;

    [SerializeField] public Transform spawnParent; // Delete this if the round ends early.

    private void Start()
    {
        GameManager.OnRoundBegin += StartRound;
        GameManager.OnGameEnd += ClearEnemies;
        enabled = false;
    }

    private void ClearEnemies()
    {
        if (spawnParent.childCount > 0)
        {
            for (int i = 0; i < spawnParent.childCount; i++)
            {
                Destroy(spawnParent.GetChild(i).gameObject);
            }
        }
    }

    private void StartRound()
    {
        ClearEnemies();
        _currentRound = rounds[Mathf.Min(GameManager.CurrentDay, rounds.Length-1)];
        DayNightCycle.DayDuration = _currentRound.DayDuration + 20;
        
        _numEnemies = 0;
        _enemyCounts = new int[_currentRound.SpawnedEnemies.Length];
        
        for (int index = 0; index < _currentRound.SpawnedEnemies.Length; index++)
        {
            RoundSO.SpawnAmount enemyType = _currentRound.SpawnedEnemies[index];
            _numEnemies += enemyType.amount;
            _enemyCounts[index] = enemyType.amount;
        }

        _timeBetweenSpawns = _currentRound.DayDuration / _numEnemies;
        enabled = true;
    }

    private void Update()
    {
        _currentSpawnPeriod += Time.deltaTime;
        if (_currentSpawnPeriod > _timeBetweenSpawns)
        {
            SpawnEnemy();
        }
    }
    
    private void SpawnEnemy()
    {

        int num = Random.Range(0, _numEnemies);
        for (int index = 0; index < _enemyCounts.Length; index++)
        {
            int val = _enemyCounts[index];
            if (num < val)
            {
                _enemyCounts[index]--;
                _numEnemies--;
                num = index; // Reuse num
                break;
            }

            num -= val;
        }

        _currentSpawnPeriod = 0;
        Instantiate(_currentRound.SpawnedEnemies[num].spawnObject, spawnPoints[Random.Range(0, spawnPoints.Length)].position, Quaternion.identity,spawnParent);

        if (_numEnemies == 0) enabled = false;

    }
}
