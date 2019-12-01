using System;
using Constants;
using Entities;
using Gameplay;
using UnityEngine;

namespace Buildings
{
    [RequireComponent(typeof(BoxCollider))]
    public class PlayerBase : MonoBehaviour
    {
        public event Action OnBaseDestroyed;
        
        private int _maxEnemiesArriveNumer;

        private int _currentEnemiesArrivedCount;
        private bool _isBaseDestroyedTriggered;

        private PlayerData _playerData;
        
        private void Awake()
        {
            _playerData = FindObjectOfType<PlayerData>();
        }

        private void Start()
        {
            _maxEnemiesArriveNumer = _playerData.PlayerBaseHealth;
            _currentEnemiesArrivedCount = 0;
            _isBaseDestroyedTriggered = false;
        }

        private void Update()
        {
            if (!_isBaseDestroyedTriggered && _currentEnemiesArrivedCount >= _maxEnemiesArriveNumer)
            {
                _isBaseDestroyedTriggered = true;   
                OnBaseDestroyed?.Invoke();
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!_isBaseDestroyedTriggered && other.transform.CompareTag(Tags.Enemy))
            {
                _currentEnemiesArrivedCount++;
            }
        }
    }
}
