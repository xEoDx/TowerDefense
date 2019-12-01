﻿using System;
using System.Collections.Generic;
using System.Linq;
using AI;
using Gameplay.States;
using Player;
using UnityEngine;
using StateMachine = FSM.StateMachine;

namespace Gameplay
{
    [RequireComponent(typeof(StateMachine))]
    public class GameplayController : MonoBehaviour
    {
        #region Events
        public event Action<Enemy> OnEnemyDead;
        public event Action<GameEndReason> OnGameEnd;
        #endregion
        
        #region Properties
        public int CurrentWaveNumber { get; private set; }
        public int TotalWavesCount { get; private set; }
        #endregion
        
        #region Fields

        public enum GameEndReason
        {
            WIN,
            LOSE
        }
        
        private StateMachine _stateStateMachine;
        private EnemySpawner _enemySpawner;
        private List<Enemy> _activeEnemies;
        private PlayerController _playerController;

        //TODO READ FROM JSON FILE
        private static Level Level;
        #endregion

        #region Unity Event Functions
        
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

            var startingIncome = 1000;

            Level = new Level(waves, startingIncome);

            TotalWavesCount = Level.Waves.Count;
            CurrentWaveNumber = 0;

            _enemySpawner = FindObjectOfType<EnemySpawner>();
        }

        private void Start()
        {
            _playerController = FindObjectOfType<PlayerController>();
            
            // Allocate enemies on Start so that we don't need to create them during game play runtime
            IDictionary<EnemySpawner.EnemyType, int> maxEnemiesPerTypeToSpawn =
                new Dictionary<EnemySpawner.EnemyType, int>();
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
        }
        #endregion

        #region Methods
       
        public IList<Enemy> GetActiveEnemies()
        {
            return _activeEnemies;
        }

        public Wave? GetCurrentWave()
        {
            return GetWave(CurrentWaveNumber);
        }

        public void IncreaseWaveNumber()
        {
            CurrentWaveNumber++;
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

        public void TriggerGameEnd(GameEndReason gameEndReason)
        {
            OnGameEnd?.Invoke(gameEndReason);
        }

        private void InitFsm()
        {
            _stateStateMachine = GetComponent<StateMachine>();

            var states = new Dictionary<Type, FSMState>
            {
                {typeof(AwaitingState), new AwaitingState(this)},
                {typeof(SpawnState), new SpawnState(this)},
                {typeof(WinState), new WinState(this)},
                {typeof(LoseState), new LoseState(this)}
            };

            _stateStateMachine.SetStates(states);
        }

        private Wave? GetWave(int waveNumber)
        {
            Wave? wave = null;

            if (waveNumber < TotalWavesCount)
            {
                wave = Level.Waves[waveNumber];
            }

            return wave;
        }

        private void UpdatedEnabledEnemiesList()
        {
            // Notify enemy died
            _activeEnemies
                .Where(e => e.IsDead() || !e.gameObject.activeSelf)
                .ToList()
                .ForEach(e => OnEnemyDead?.Invoke(e));

            // Remove from list
            _activeEnemies
                .RemoveAll(e => e.IsDead() || !e.gameObject.activeSelf);
        }

        #endregion

    }
}