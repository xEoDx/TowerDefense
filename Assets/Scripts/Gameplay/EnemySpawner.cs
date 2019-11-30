using System;
using System.Collections.Generic;
using UnityEngine;

namespace Gameplay
{
    public class EnemySpawner : MonoBehaviour
    {
        private IDictionary<EnemyType, IList<GameObject>> _spawnedEnemies;

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
            _spawnedEnemies = new Dictionary<EnemyType, IList<GameObject>>(enemiesToSpawn.Count);
            foreach (var kvp in enemiesToSpawn)
            {
                IList<GameObject> instantiatedEnemyPrefabsList = new List<GameObject>(kvp.Value);
                for (int i = 0; i < kvp.Value; i++)
                {
                    var prefabPath = _enemyToPrefabPathDictionary[kvp.Key];
                    var enemy = Instantiate(Resources.Load<GameObject>(prefabPath));
                    instantiatedEnemyPrefabsList.Add(enemy);
                }
                _spawnedEnemies.Add(kvp.Key, instantiatedEnemyPrefabsList);
            }
        }
    }
}
