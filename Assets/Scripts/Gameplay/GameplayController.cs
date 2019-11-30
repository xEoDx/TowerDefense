using System;
using System.Collections.Generic;
using AI;
using Gameplay.States;
using UnityEngine;
using StateMachine = FSM.StateMachine;

namespace Gameplay
{
    [RequireComponent(typeof(StateMachine))]
    public class GameplayController : MonoBehaviour
    {
        private StateMachine _stateStateMachine;
        
        //TODO READ FROM JSON FILE
        private static Level Level;

        private EnemySpawner _enemySpawner;
        
        private int _currentWaveNumber;

        public int CurrentWaveNumber => _currentWaveNumber;

        private int _totalWavesCount;
        public int TotalWavesCount => _totalWavesCount;

        private List<Enemy> _activeEnemies;

        public List<Enemy> ActiveEnemies => _activeEnemies;
    
        private void Awake()
        {
            InitFsm();
            
            _activeEnemies = new List<Enemy>();
            
            IList<Wave> waves = new List<Wave>();
            IDictionary<EnemySpawner.EnemyType, int> waveEnemies = new Dictionary<EnemySpawner.EnemyType, int>()
            {
                {EnemySpawner.EnemyType.Basic, 2},
                {EnemySpawner.EnemyType.Big, 2}
            };
            float timeToSpawn = 5;

            Wave wave = new Wave(waveEnemies, timeToSpawn);
            waves.Add(wave);

            waveEnemies = new Dictionary<EnemySpawner.EnemyType, int>
            {
                {EnemySpawner.EnemyType.Basic, 5},
                {EnemySpawner.EnemyType.Fast, 2},
                {EnemySpawner.EnemyType.Big, 2}
            };
            timeToSpawn = 15;

            wave = new Wave(waveEnemies, timeToSpawn);
            waves.Add(wave);

            int startingIncome = 1000;

            Level = new Level(waves, startingIncome);

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

        private void InitFsm()
        {
            _stateStateMachine = GetComponent<StateMachine>();
        
            var states = new Dictionary<Type, FSMState>
            {
                {typeof(AwaitingState), new AwaitingState(this)},
                {typeof(SpawnState), new SpawnState(this)}
            };
        
            _stateStateMachine.SetStates(states);
        }

        private void Update()
        {
            UpdatedEnabledEnemiesList();

        }

        public IList<Enemy> GetActiveEnemies()
        {
            return _activeEnemies;
        }

        public Wave? GetCurrentWave()
        {
            return GetWave(_currentWaveNumber);
        }

        public void IncreaseWaveNumber()
        {
            _currentWaveNumber++;
        }

        public void Spawn(EnemySpawner.EnemyType type, int amount)
        {
            var enabledEnemies = _enemySpawner.Enable(type, amount);
            _activeEnemies.AddRange(enabledEnemies);
        }

        public int GetStartingIncome()
        {
            return Level.InitialIncome;
        }


        private Wave? GetWave(int waveNumber)
        {
            Wave? wave = null;
            
            if (waveNumber < _totalWavesCount)
            {
                wave = Level.Waves[waveNumber];
            }
            
            return wave;
        }

        private void UpdatedEnabledEnemiesList()
        {
            _activeEnemies.RemoveAll(e => e.IsDead() || !e.gameObject.activeSelf);
        }
    }
}
