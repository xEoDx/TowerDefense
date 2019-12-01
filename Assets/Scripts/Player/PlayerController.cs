using System;
using AI;
using Gameplay;
using UnityEngine;

namespace Player
{
    public class PlayerController : MonoBehaviour
    {
        public event Action<int> OnIncomeUpdated;

        public PlayerGameplayData PlayerGameplayData { get; private set; }
        private GameplayController _gameplayController;

        private void Awake()
        {
            _gameplayController = FindObjectOfType<GameplayController>();
        }

        private void Start()
        {
            _gameplayController.OnEnemyDead += OnEnemyDeadListener;
            
            AddIncome(_gameplayController.GetStartingIncome());
        }

        public void SubtractIncome(int amount)
        {
            var updatedIncome = PlayerGameplayData.SubtractIncome(amount);
            OnIncomeUpdated?.Invoke(updatedIncome);
        }

        private void AddIncome(int amount)
        {
            var updatedIncome = PlayerGameplayData.AddIncome(amount);
            OnIncomeUpdated?.Invoke(updatedIncome);
        }

        private void OnEnemyDeadListener(Enemy enemy)
        {
            AddIncome(enemy.Reward);
        }
    }
}
