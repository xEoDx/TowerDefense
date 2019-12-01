using System;
using Constants;
using Entities;
using UnityEngine;

namespace Buildings
{
    [RequireComponent(typeof(BoxCollider))]
    public class PlayerBase : MonoBehaviour
    {
        public event Action OnBaseDestroyed;
        
        [SerializeField] private int maxEnemiesArriveNumer = 5;

        private int _currentEnemiesArrivedCount;
        private bool _isBaseDestroyedTriggered;

        private void Start()
        {
            _currentEnemiesArrivedCount = 0;
            _isBaseDestroyedTriggered = false;
        }

        private void Update()
        {
            if (!_isBaseDestroyedTriggered && _currentEnemiesArrivedCount >= maxEnemiesArriveNumer)
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
