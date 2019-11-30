using System;
using System.Collections.Generic;
using UnityEngine;
using StateMachine = FSM.StateMachine;

namespace Gameplay
{
    [RequireComponent(typeof(StateMachine))]
    public class GameplayController : MonoBehaviour
    {
        private StateMachine _stateStateMachine;
        private EnemySpawner _enemySpawner;
    
        private void Awake()
        {
            InitFSM();
        }

        private void Start()
        {
            _enemySpawner = FindObjectOfType<EnemySpawner>();
            IDictionary<EnemySpawner.EnemyType, int> enemiesToSpawn = new Dictionary<EnemySpawner.EnemyType, int>()
            {
                {EnemySpawner.EnemyType.Basic, 5},
                {EnemySpawner.EnemyType.Fast, 1},
                {EnemySpawner.EnemyType.Big, 1}
            };
            _enemySpawner.Init(enemiesToSpawn);
        }

        private void InitFSM()
        {
            _stateStateMachine = GetComponent<StateMachine>();
        
            var states = new Dictionary<Type, FSMState>
            {
                {typeof(StartingState), new StartingState(this)},
                {typeof(PlayingState), new PlayingState(this)}
            };
        
            _stateStateMachine.SetStates(states);
        }
    }
}
