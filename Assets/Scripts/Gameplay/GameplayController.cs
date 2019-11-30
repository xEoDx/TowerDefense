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
        private LevelController _levelController;
    
        private void Awake()
        {
            InitFsm();
        }

        private void Start()
        {
            _levelController = FindObjectOfType<LevelController>();
        }

        private void InitFsm()
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
