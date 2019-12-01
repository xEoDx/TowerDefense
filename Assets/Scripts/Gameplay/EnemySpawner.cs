using System;
using System.Collections.Generic;
using AI;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Gameplay
{
    public class EnemySpawner : MonoBehaviour
    {
        [SerializeField] private GameObject[] spawnPoints;
        
        private IDictionary<EnemyType, IList<Enemy>> _spawnedEnemies;


        public enum EnemyType
        {
            Basic,
            Fast,
            Big
        }
        private readonly IDictionary<EnemyType, string> _enemyToPrefabPathDictionary = new Dictionary<EnemyType, string>
        {
            {EnemyType.Basic, "Prefabs/BasicEnemy"},
            {EnemyType.Fast, "Prefabs/FastEnemy"},
            {EnemyType.Big, "Prefabs/BigEnemy"}
        };


        public void Init(IDictionary<EnemyType, int> enemiesToSpawn)
        {
            _spawnedEnemies = new Dictionary<EnemyType, IList<Enemy>>(enemiesToSpawn.Count);
            foreach (var kvp in enemiesToSpawn)
            {
                IList<Enemy> instantiatedEnemyPrefabsList = new List<Enemy>(kvp.Value);
                for (int i = 0; i < kvp.Value; i++)
                {
                    var prefabPath = _enemyToPrefabPathDictionary[kvp.Key];
                    var enemy = Instantiate(Resources.Load<GameObject>(prefabPath));
                    enemy.SetActive(false);
                    instantiatedEnemyPrefabsList.Add(enemy.GetComponent<Enemy>());
                }
                _spawnedEnemies.Add(kvp.Key, instantiatedEnemyPrefabsList);
            }
        }

        private Vector3 GetRandomPosition()
        {
            int index = Random.Range(0, spawnPoints.Length - 1);
            return spawnPoints[index].transform.position;
        }

        public IList<Enemy> Enable(EnemyType type, int amount)
        {
            IList<Enemy> enemies = new List<Enemy>(amount);
            var enemiesFromType = _spawnedEnemies[type];
            if (enemiesFromType.Count < amount)
            {
                amount = enemiesFromType.Count;
            }

            var foundEnemies = 0;
            var enemiesOfType = _spawnedEnemies[type];
            
            foreach (var enemy in enemiesOfType)
            {
                if (foundEnemies < amount)
                {
                    if (!enemy.gameObject.activeSelf || enemy.IsDead())
                    {
                        enemy.Reset();
                        enemy.transform.position = GetRandomPosition();
                        enemy.gameObject.SetActive(true);
                        enemies.Add(enemy);
                        foundEnemies++;
                    }
                }
            }

            return enemies;
        }
    }
}
