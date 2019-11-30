using System;
using AI;
using Gameplay;
using UnityEngine;

namespace Player
{
    public class PlayerController : MonoBehaviour
    {
        public event Action<int> OnIncomeUpdated;
        
        [SerializeField] 
        private PlayerGameplayData playerGameplayData;

        public PlayerGameplayData PlayerGameplayData => playerGameplayData;

        private GameplayController _gameplayController;

        private void Start()
        {
            _gameplayController = FindObjectOfType<GameplayController>();
            _gameplayController.OnEnemyDead += OnEnemyDeadListener;
            
            AddIncome(_gameplayController.GetStartingIncome());
        }

        private void OnEnemyDeadListener(Enemy enemy)
        {
            AddIncome(enemy.Reward);
        }

        public void AddIncome(int amount)
        {
            var updatedIncome = playerGameplayData.AddIncome(amount);
            OnIncomeUpdated?.Invoke(updatedIncome);
        }

        public void SubtractIncome(int amount)
        {
            var updatedIncome = playerGameplayData.SubtractIncome(amount);
            OnIncomeUpdated?.Invoke(updatedIncome);
        }
    }
}
