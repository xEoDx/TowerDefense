using System.Collections;
using System.Collections.Generic;
using AI;
using Gameplay;
using UnityEngine;

public class LevelController : MonoBehaviour
{
    //TODO READ FROM JSON FILE
    private static Level Level;

    private EnemySpawner _enemySpawner;
    private float _elapsedTime;
    private int _currentWaveNumber;

    public int CurrentWaveNumber => _currentWaveNumber;

    private int _totalWavesCount;
    public int TotalTotalWavesCount => _totalWavesCount;

    private List<Enemy> _activeEnemies;

    public List<Enemy> ActiveEnemies => _activeEnemies;


    private void Awake()
    {
        _activeEnemies = new List<Enemy>();

        _elapsedTime = 0;
        IList<Wave> waves = new List<Wave>();
        IDictionary<EnemySpawner.EnemyType, int> waveEnemies = new Dictionary<EnemySpawner.EnemyType, int>()
        {
            {EnemySpawner.EnemyType.Basic, 2},
            {EnemySpawner.EnemyType.Big, 2}
        };
        float timeToSpawn = 10;

        Wave wave = new Wave(waveEnemies, timeToSpawn);
        waves.Add(wave);

        waveEnemies = new Dictionary<EnemySpawner.EnemyType, int>()
        {
            {EnemySpawner.EnemyType.Basic, 5},
            {EnemySpawner.EnemyType.Fast, 2},
            {EnemySpawner.EnemyType.Big, 2}
        };
        timeToSpawn = 15;

        wave = new Wave(waveEnemies, timeToSpawn);
        waves.Add(wave);

        Level = new Level(waves);

        _totalWavesCount = Level.Waves.Count;
        _currentWaveNumber = 0;

        _enemySpawner = FindObjectOfType<EnemySpawner>();
    }

    private void Start()
    {
        IDictionary<EnemySpawner.EnemyType, int> maxEnemiesPerTypeToSpawn = new Dictionary<EnemySpawner.EnemyType, int>();
        foreach (var wave in Level.Waves)
        {
            foreach (var kvp in wave.WaveEnemies)
            {
                if (!maxEnemiesPerTypeToSpawn.ContainsKey(kvp.Key))
                {
                    maxEnemiesPerTypeToSpawn.Add(kvp.Key, kvp.Value);
                }
                else
                {
                    var currentAmount = maxEnemiesPerTypeToSpawn[kvp.Key];
                    var updatedEnemyCount = currentAmount + kvp.Value;
                    maxEnemiesPerTypeToSpawn[kvp.Key] = updatedEnemyCount;
                }
            }
        }
        _enemySpawner.Init(maxEnemiesPerTypeToSpawn);
    }

    private void Update()
    {
        UpdatedEnabledEnemiesList();
        if (_currentWaveNumber < _totalWavesCount)
        {
            if (_elapsedTime > Level.Waves[_currentWaveNumber].TimeToSpawn)
            {
                _elapsedTime = 0;

                var waveEnemies = Level.Waves[_currentWaveNumber].WaveEnemies;

                foreach (var kvp in waveEnemies)
                {
                    var enabledEnemies = _enemySpawner.Enable(kvp.Key, kvp.Value);
                    _activeEnemies.AddRange(enabledEnemies);
                }

                _currentWaveNumber++;
                // TODO launch new wave event
            }
            _elapsedTime += Time.deltaTime;
        }
        else
        {
            // TODO set gameplay completed
        }

    }

    private void UpdatedEnabledEnemiesList()
    {
        _activeEnemies.RemoveAll(e => e.IsDead() || !e.gameObject.activeSelf);
    }
}