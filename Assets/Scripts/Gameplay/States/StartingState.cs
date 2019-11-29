using System;
using UnityEngine;

public class StartingState : FSMState
{
    private GameplayController _owner;

    public StartingState(GameplayController owner) : base(owner.gameObject)
    {
        _owner = owner;
    }

    public override void Init()
    {
        Debug.Log("[StartingState] OnEnter");
    }

    public override Type Execute()
    {
        Debug.Log("[StartingState] OnUpdate");
        //todo compute logic
        return typeof(PlayingState);
    }

    public override void Exit()
    {
        Debug.Log("[StartingState] OnExit");
    }
}
