using System;
using FSM;

namespace Gameplay.States
{
    public class LoseState : FsmState
    {
        private readonly GameplayController _gameplayController;
        public LoseState(GameplayController gameplayController) : base(gameplayController)
        {
            _gameplayController = gameplayController;
        }

        public override void Init()
        {
            _gameplayController.TriggerGameEnd(GameplayController.GameEndReason.Lose);
        }

        public override Type Execute()
        {
            return typeof(LoseState);
        }

        public override void Exit()
        {}
    }
}
