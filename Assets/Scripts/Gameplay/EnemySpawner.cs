using System;
using System.Collections.Generic;
using System.Linq;
using AI;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Gameplay
{
    public class EnemySpawner : MonoBehaviour
    {
        [SerializeField] private GameObject[] spawnPoints;
        
        private IDictionary<EnemyType, IList<Enemy>> _spawnedEnemies;

        private IList<Enemy> _activeEnemies;

        public enum EnemyType
        {
            Basic,
            Fast,
            Big
        }
        private readonly IDictionary<EnemyType, String> _enemyToPrefabPathDictionary = new Dictionary<EnemyType, string>()
        {
            {EnemyType.Basic, "Prefabs/BasicEnemy"},
            {EnemyType.Fast, "Prefabs/BasicEnemy"},
            {EnemyType.Big, "Prefabs/BasicEnemy"}
        };


        public void Init(IDictionary<EnemyType, int> enemiesToSpawn)
        {
            _activeEnemies = new List<Enemy>();
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

        public void Enable(EnemyType type, int amount)
        {
            _activeEnemies.Clear();
            
            var enemiesFromType = _spawnedEnemies[type];
            if (enemiesFromType.Count < amount)
            {
                amount = enemiesFromType.Count;
            }

            for (int i = 0; i < amount; i++)
            {
                var enemy = _spawnedEnemies[type][i];
                enemy.gameObject.SetActive(true);
                enemy.transform.position = GetRandomPosition();
                _activeEnemies.Add(enemy);
            }
        }

        public IList<Enemy> GetActiveEnemies()
        {
            return _activeEnemies;
        }
    }
}
