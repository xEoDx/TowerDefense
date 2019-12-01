using System;
using System.Collections.Generic;
using System.Linq;
using AI;
using Buildings;
using FSM;
using Gameplay.States;
using Player;
using UnityEngine;
using StateMachine = FSM.StateMachine;

namespace Gameplay
{
    [RequireComponent(typeof(StateMachine))]
    [RequireComponent(typeof(LevelData))]
    public class GameplayController : MonoBehaviour
    {
        #region Events
        public event Action<BasicEnemy> OnEnemyDead;
        public event Action<GameEndReason> OnGameEnd;
        #endregion
        
        #region Properties
        public int CurrentWaveNumber { get; private set; }
        public int TotalWavesCount { get; private set; }
        public bool ShouldTriggerGameLose { get; private set; }
        #endregion
        
        #region Fields

        public enum GameEndReason
        {
            Win,
            Lose
        }
        
        private StateMachine _stateStateMachine;
        private EnemySpawner _enemySpawner;
        private List<BasicEnemy> _activeEnemies;
        private PlayerBase _playerBase;
        private LevelData _levelData;
        #endregion

        #region Unity Event Functions
        
        private void Awake()
        {
            _levelData = GetComponent<LevelData>();
            _playerBase = FindObjectOfType<PlayerBase>();
        }

        private void Start()
        {
            InitFsm();
            InitLevelData();

            _playerBase.OnBaseDestroyed += OnPlayerBaseDestroyed;
            
            // Allocate enemies on Start so that we don't need to create them during game play runtime
            IDictionary<EnemyType, int> maxEnemiesPerTypeToSpawn =
                new Dictionary<EnemyType, int>();
            foreach (var wave in _levelData.Level.Waves)
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


        public IList<BasicEnemy> GetActiveEnemies()
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

        public void Spawn(EnemyType type, int amount)
        {
            var enabledEnemies = _enemySpawner.Enable(type, amount);
            _activeEnemies.AddRange(enabledEnemies);
        }

        public int GetStartingIncome()
        {
            return _levelData.Level.InitialIncome;
        }

        public void TriggerGameEnd(GameEndReason gameEndReason)
        {
            OnGameEnd?.Invoke(gameEndReason);
        }

        private void InitFsm()
        {
            _stateStateMachine = GetComponent<StateMachine>();

            var states = new Dictionary<Type, FsmState>
            {
                {typeof(AwaitingState), new AwaitingState(this)},
                {typeof(SpawnState), new SpawnState(this)},
                {typeof(WinState), new WinState(this)},
                {typeof(LoseState), new LoseState(this)}
            };

            _stateStateMachine.SetStates(states, typeof(AwaitingState));
        }
        
        private void InitLevelData()
        {
            TotalWavesCount = _levelData.Level.Waves.Count;
            _activeEnemies = new List<BasicEnemy>();
            CurrentWaveNumber = 0;
            _enemySpawner = FindObjectOfType<EnemySpawner>();
        }

        private Wave? GetWave(int waveNumber)
        {
            Wave? wave = null;

            if (waveNumber < TotalWavesCount)
            {
                wave = _levelData.Level.Waves[waveNumber];
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
        
        private void OnPlayerBaseDestroyed()
        {
            ShouldTriggerGameLose = true;
        }

        #endregion


    }
}
