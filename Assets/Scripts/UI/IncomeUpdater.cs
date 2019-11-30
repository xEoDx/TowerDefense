using System;
using Player;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class IncomeUpdater : MonoBehaviour
    {
        [SerializeField] 
        private Text incomeText;

        [SerializeField] private float textUpdateSpeed = 2;
        
        private PlayerController _playerController;
        private int _currentIncome;
        private int _updatedIncome;

        private bool _isInitialized;

        private void Start()
        {
            _isInitialized = false;
            _playerController = FindObjectOfType<PlayerController>();
            _playerController.OnIncomeUpdated += OnIncomeUpdatedListener;
        }

        private void OnIncomeUpdatedListener(int updatedIncome)
        {
            if (!_isInitialized)
            {
                InitializeText(updatedIncome);
            }
            
            _updatedIncome = updatedIncome;
        }

        private void InitializeText(int updatedIncome)
        {
            _isInitialized = true;
            _currentIncome = updatedIncome;
            incomeText.text = _currentIncome.ToString();
        }

        private void Update()
        {
            if (_currentIncome != _updatedIncome)
            {
                _currentIncome = (int) Mathf.Lerp(_currentIncome, _updatedIncome, textUpdateSpeed * Time.deltaTime);
                incomeText.text = _currentIncome.ToString();
            }
        }
    }
}
