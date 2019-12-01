using System;
using FSM;

namespace Gameplay.States
{
    public class SpawnState : FsmState
    {
        private readonly GameplayController _gameplayController;

        public SpawnState(GameplayController gameplayController) : base(gameplayController)
        {
            _gameplayController = gameplayController;
        }

        public override void Init()
        {
            var waveEnemies = _gameplayController.GetCurrentWave()?.WaveEnemies;

            if (waveEnemies != null)
            {
                foreach (var kvp in waveEnemies)
                {
                    _gameplayController.Spawn(kvp.Key, kvp.Value);
                }
                _gameplayController.IncreaseWaveNumber();
            }
        }

        public override Type Execute()
        {
            return typeof(AwaitingState);
        }

        public override void Exit()
        {
        }
    }
}
