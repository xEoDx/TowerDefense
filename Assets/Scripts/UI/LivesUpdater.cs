using Buildings;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class LivesUpdater : MonoBehaviour
    {
        public Text livesText;

        private PlayerBase _playerBase;

        private void Awake()
        {
            _playerBase = FindObjectOfType<PlayerBase>();
        }

        void Start()
        {
            _playerBase.OnPlayerLivesUpdated += OnLivesUpdatedListener;
        }

        private void OnLivesUpdatedListener(int amount)
        {
            livesText.text = amount.ToString();
        }
    }
}
