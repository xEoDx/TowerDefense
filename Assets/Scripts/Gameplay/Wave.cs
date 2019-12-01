using System.Collections.Generic;

namespace Gameplay
{
    public struct Wave
    {
        private IDictionary<AI.EnemyType, int> _waveEnemies;
        public IDictionary<AI.EnemyType, int> WaveEnemies => _waveEnemies;
        
        private float _timeToSpawn;
        public float TimeToSpawn => _timeToSpawn;

        public Wave(IDictionary<AI.EnemyType, int> waveEnemies, float timeToSpawn)
        {
            _waveEnemies = waveEnemies;
            _timeToSpawn = timeToSpawn;
        }
        
        
    }
}