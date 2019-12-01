using System;

namespace Gameplay.States
{
    public class LoseState : FSMState
    {
        private GameplayController _gameplayController;
        public LoseState(GameplayController gameplayController) : base(gameplayController.gameObject)
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
