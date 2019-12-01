using System;
using FSM;
using UnityEngine;

namespace Gameplay.States
{
    public class AwaitingState : FsmState
    {
        private readonly GameplayController _gameplayController;
        private float _elapsedTime;
    
        public AwaitingState(GameplayController gameplayController) : base(gameplayController)
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
            _elapsedTime = 0;
        }
    }
}
