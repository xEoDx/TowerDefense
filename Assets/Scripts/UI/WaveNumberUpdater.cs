using Gameplay;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class WaveNumberUpdater : MonoBehaviour
    {
        [SerializeField] private Text waveNumberText;

        private GameplayController _gameplayController;
        private int _currentWave;

        private void Awake()
        {
            _gameplayController = FindObjectOfType<GameplayController>();
            _currentWave = _gameplayController.CurrentWaveNumber;
        }

        private void Start()
        {
            UpdateWaveNumber();
        }

        private void Update()
        {
            if (_currentWave != _gameplayController.CurrentWaveNumber)
            {
                UpdateWaveNumber();
            }
        }

        private void UpdateWaveNumber()
        {
            _currentWave = _gameplayController.CurrentWaveNumber;
            var waveNumberStr = _currentWave + "/" + _gameplayController.TotalWavesCount;
            waveNumberText.text = waveNumberStr;
        }
    }
}