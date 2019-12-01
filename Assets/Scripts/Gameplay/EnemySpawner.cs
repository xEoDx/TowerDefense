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
        
        private IDictionary<EnemyType, IList<BasicEnemy>> _spawnedEnemies;
        
        private readonly IDictionary<EnemyType, string> _enemyToPrefabPathDictionary = new Dictionary<EnemyType, string>
        {
            {EnemyType.Basic, "Prefabs/BasicEnemy"},
            {EnemyType.Fast, "Prefabs/FastEnemy"},
            {EnemyType.Big, "Prefabs/BigEnemy"}
        };


        public void Init(IDictionary<EnemyType, int> enemiesToSpawn)
        {
            _spawnedEnemies = new Dictionary<EnemyType, IList<BasicEnemy>>(enemiesToSpawn.Count);
            foreach (var kvp in enemiesToSpawn)
            {
                IList<BasicEnemy> instantiatedEnemyPrefabsList = new List<BasicEnemy>(kvp.Value);
                for (int i = 0; i < kvp.Value; i++)
                {
                    var prefabPath = _enemyToPrefabPathDictionary[kvp.Key];
                    var enemy = Instantiate(Resources.Load<GameObject>(prefabPath));
                    enemy.SetActive(false);
                    instantiatedEnemyPrefabsList.Add(enemy.GetComponent<BasicEnemy>());
                }
                _spawnedEnemies.Add(kvp.Key, instantiatedEnemyPrefabsList);
            }
        }

        private Vector3 GetRandomPosition()
        {
            int index = Random.Range(0, spawnPoints.Length);
            return spawnPoints[index].transform.position;
        }

        public IList<BasicEnemy> Enable(EnemyType type, int amount)
        {
            IList<BasicEnemy> enemies = new List<BasicEnemy>(amount);
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
