﻿using System;
using Gameplay;
using UnityEngine;

public class PlayingState : FSMState
{
    private GameplayController _gameplayController;

    public PlayingState(GameplayController owner) : base(owner.gameObject)
    {
        _gameplayController = owner;
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
