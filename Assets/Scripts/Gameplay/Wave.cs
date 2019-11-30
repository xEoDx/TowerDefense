using System.Collections.Generic;

namespace Gameplay
{
    public struct Wave
    {
        private IDictionary<EnemySpawner.EnemyType, int> _waveEnemies;
        public IDictionary<EnemySpawner.EnemyType, int> WaveEnemies => _waveEnemies;
        
        private float _timeToSpawn;
        public float TimeToSpawn => _timeToSpawn;

        public Wave(IDictionary<EnemySpawner.EnemyType, int> waveEnemies, float timeToSpawn)
        {
            _waveEnemies = waveEnemies;
            _timeToSpawn = timeToSpawn;
        }
        
        
    }
}