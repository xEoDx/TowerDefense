using System;
using UnityEngine;

namespace Gameplay.States
{
    public class WinState : FSMState
    {
        private GameplayController _gameplayController;
        public WinState(GameplayController gameplayController) : base(gameplayController.gameObject)
        {
            _gameplayController = gameplayController;
        }

        public override void Init()
        {
            _gameplayController.TriggerGameEnd(GameplayController.GameEndReason.WIN);
        }

        public override Type Execute()
        {
            return typeof(WinState);
        }

        public override void Exit()
        {}
    }
}
