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
            if (_gameplayController.CurrentWaveNumber < _gameplayController.TotalWavesCount)
            {
                if (_elapsedTime > _gameplayController.GetCurrentWave()?.TimeToSpawn)
                {
                    return typeof(SpawnState);
                }
                
                _elapsedTime += Time.deltaTime;
                return typeof(AwaitingState);
            }

            //TODO all rounds complete = End state
            return typeof(AwaitingState);
        }

        public override void Exit()
        {
            Debug.Log("[StartingState] OnExit");
            _elapsedTime = 0;
        }
    }
}
