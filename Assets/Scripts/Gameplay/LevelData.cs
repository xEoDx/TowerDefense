using System;
using System.Collections.Generic;
using AI;
using Buildings;
using UnityEngine;

namespace Gameplay
{
    [Serializable]
    public class LevelData : MonoBehaviour
    {
        [SerializeField] 
        private int initialIncome = 1000;
    
        [SerializeField]
        private List<WavesData> wavesData;

        [SerializeField] private EntityAttributes basicEnemyAttributes;
        public EntityAttributes BasicEnemyAttributes => basicEnemyAttributes;

        [SerializeField] private EntityAttributes fastEnemyAttributes;

        public EntityAttributes FastEnemyAttributes => fastEnemyAttributes;

        [SerializeField] private EntityAttributes bigEnemyAttributes;
        public EntityAttributes BigEnemyAttributes => bigEnemyAttributes;


        private Level _level;
        public Level Level => _level;

        private void Awake()
        {
            IList<Wave> waves = new List<Wave>();
            Wave w;
        
            foreach (var wave in wavesData)
            {
                IDictionary<EnemyType, int> enemyWave = new Dictionary<EnemyType, int>();
                foreach (var waveEnemies in wave.wave)
                {
                    enemyWave.Add(waveEnemies.type, waveEnemies.amount);
                }
                w = new Wave(enemyWave, wave.timeToSpawn);
                waves.Add(w);
            }
        
            _level = new Level(waves, initialIncome);
        }

        public EntityAttributes GetEnemyAttributes(EnemyType enemyType)
        {
            EntityAttributes entityAttributes = new EntityAttributes();
            
            switch (enemyType)
            {
                case EnemyType.Basic:
                    entityAttributes = basicEnemyAttributes;
                    break;
                case EnemyType.Fast:
                    entityAttributes = fastEnemyAttributes;
                    break;
                case EnemyType.Big:
                    entityAttributes = bigEnemyAttributes;
                    break;
                default:
                    Debug.Log("[LevelData] Unclassified EnemyType. Can't fetch enemy attributes");
                    break;
            }

            return entityAttributes;
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
        public EnemyType type;
        public int amount;
    }
}