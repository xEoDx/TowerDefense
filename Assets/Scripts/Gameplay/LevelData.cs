using System;
using System.Collections;
using System.Collections.Generic;
using Gameplay;
using UnityEngine;

[Serializable]
public class LevelData : MonoBehaviour
{
    [SerializeField] 
    private int initialIncome = 1000;
    
    [SerializeField]
    private List<WavesData> wavesData;

    private Level _level;
    public Level Level => _level;


    private void Awake()
    {
        IList<Wave> waves = new List<Wave>();
        Wave w;
        
        foreach (var wave in wavesData)
        {
            IDictionary<EnemySpawner.EnemyType, int> enemyWave = new Dictionary<EnemySpawner.EnemyType, int>();
            foreach (var waveEnemies in wave.wave)
            {
                    enemyWave.Add(waveEnemies.type, waveEnemies.amount);
            }
            w = new Wave(enemyWave, wave.timeToSpawn);
            waves.Add(w);
        }
        
        _level = new Level(waves, initialIncome);

    }
}

[Serializable]
public struct WavesData
{
    [SerializeField]
    public List<WaveData> wave;

    [SerializeField]
    public float timeToSpawn;
}

[Serializable]
public struct WaveData
{
    public EnemySpawner.EnemyType type;
    public int amount;
}
