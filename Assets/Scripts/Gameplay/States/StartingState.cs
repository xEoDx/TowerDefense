using System;
using Gameplay;
using UnityEngine;

public class StartingState : FSMState
{
    private GameplayController _owner;
    
    //todo check from gameplaycontroller
    private const float AwaitTime = 5.0f;
    private float _elapsedTime;
    
    public StartingState(GameplayController owner) : base(owner.gameObject)
    {
        _owner = owner;
    }

    public override void Init()
    {
        _elapsedTime = 0;
        
    }

    public override Type Execute()
    {
        if (_elapsedTime > AwaitTime)
        {
            return typeof(PlayingState);
        }
        
        _elapsedTime += Time.deltaTime;
        return typeof(StartingState);
    }

    public override void Exit()
    {
        Debug.Log("[StartingState] OnExit");
    }
}
