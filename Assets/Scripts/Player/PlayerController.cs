using System;
using AI;
using Gameplay;
using UnityEngine;

namespace Player
{
    public class PlayerController : MonoBehaviour
    {
        public event Action<int> OnIncomeUpdated;

        private PlayerGameplayData _playerGameplayData;
        private GameplayController _gameplayController;

        private bool _isInitialSeedingDone;

        private void Awake()
        {
            _playerGameplayData = new PlayerGameplayData(0);
            _isInitialSeedingDone = false;
            _gameplayController = FindObjectOfType<GameplayController>();
        }

        private void Start()
        {
            _gameplayController.OnEnemyDead += OnEnemyDeadListener;
        }

        private void Update()
        {
            if (!_isInitialSeedingDone)
            {
                _isInitialSeedingDone = true;
                AddIncome(_gameplayController.GetStartingIncome());
            }
        }

        public void SubtractIncome(int amount)
        {
            var updatedIncome = _playerGameplayData.SubtractIncome(amount);
            OnIncomeUpdated?.Invoke(updatedIncome);
        }

        private void AddIncome(int amount)
        {
            var updatedIncome = _playerGameplayData.AddIncome(amount);
            OnIncomeUpdated?.Invoke(updatedIncome);
        }

        private void OnEnemyDeadListener(BasicEnemy basicEnemy)
        {
            AddIncome(basicEnemy.Reward);
        }
    }
}
