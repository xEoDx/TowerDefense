using System;
using System.Collections.Generic;
using UnityEngine;
using StateMachine = FSM.StateMachine;

[RequireComponent(typeof(StateMachine))]
public class GameplayController : MonoBehaviour
{
    private StateMachine _stateStateMachine;

    private void Awake()
    {
        InitFSM();
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
