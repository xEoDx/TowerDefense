using System;
using Gameplay;
using UnityEngine;

public class PlayingState : FSMState
{
    private GameplayController _stateMachine;

    public PlayingState(GameplayController owner) : base(owner.gameObject)
    {
        _stateMachine = owner;
    }


    public override void Init()
    {
    }

    public override Type Execute()
    {
        return typeof(PlayingState);
        //todo compute logic
    }

    public override void Exit()
    {
    }
}
