using Gameplay;
using UnityEngine;

namespace Player
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private PlayerGameplayData playerGameplayData;

        public PlayerGameplayData PlayerGameplayData => playerGameplayData;

        private GameplayController _gameplayController;

        private void Start()
        {
            _gameplayController = FindObjectOfType<GameplayController>();
            playerGameplayData.AddIncome(_gameplayController.GetStartingIncome());
        }

        public void SubtractIncome(int amount)
        {
            playerGameplayData.SubtractIncome(amount);
        }
    }
}
