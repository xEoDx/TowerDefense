using System;

namespace Gameplay.States
{
    public class SpawnState : FSMState
    {
        private GameplayController _gameplayController;

        public SpawnState(GameplayController gameplayController) : base(gameplayController.gameObject)
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
