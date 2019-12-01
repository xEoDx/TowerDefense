using System;
using UnityEngine;

namespace Gameplay.States
{
    public class AwaitingState : FSMState
    {
        private GameplayController _gameplayController;
        private float _elapsedTime;
    
        public AwaitingState(GameplayController gameplayController) : base(gameplayController.gameObject)
        {
            _gameplayController = gameplayController;
        }

        public override void Init()
        {
        }

        public override Type Execute()
        {
            if (_gameplayController.ShouldTriggerGameLose)
            {
                return typeof(LoseState);
            }
            
            if (_gameplayController.CurrentWaveNumber < _gameplayController.TotalWavesCount)
            {
                if (_elapsedTime > _gameplayController.GetCurrentWave()?.TimeToSpawn)
                {
                    return typeof(SpawnState);
                }
                
                _elapsedTime += Time.deltaTime;
            }
            else if (_gameplayController.GetActiveEnemies().Count == 0)
            {
                return typeof(WinState);
            }

            return typeof(AwaitingState);
        }

        public override void Exit()
        {
            Debug.Log("[StartingState] OnExit");
            _elapsedTime = 0;
        }
    }
}
