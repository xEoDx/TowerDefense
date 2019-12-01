using System;
using FSM;
using UnityEngine;

namespace Gameplay.States
{
    public class WinState : FsmState
    {
        private readonly GameplayController _gameplayController;
        public WinState(GameplayController gameplayController) : base(gameplayController)
        {
            _gameplayController = gameplayController;
        }

        public override void Init()
        {
            _gameplayController.TriggerGameEnd(GameplayController.GameEndReason.Win);
        }

        public override Type Execute()
        {
            return typeof(WinState);
        }

        public override void Exit()
        {}
    }
}
