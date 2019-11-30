using System;
using AI;
using Ammo;
using UnityEngine;
using UnityEngine.Experimental.PlayerLoop;

namespace Buildings.States
{
    public class AttackState : FSMState
    {
        private Tower _tower;
        private AmmoPool _ammoPool;
        private Enemy _currentTarget;
        private Transform _enemyTransform;
        private float _attackElapsedTime;
        public AttackState(Tower tower) : base(tower.gameObject)
        {
            _tower = tower;
            _ammoPool = _tower.AmmoPool;
        }

        public override void Init()
        {
            _attackElapsedTime = 0;
            _currentTarget = _tower.GetCurrentTarget();
            _enemyTransform = _tower.GetCurrentTarget().transform;

        }

        public override Type Execute()
        {
            if (_currentTarget.IsDead() || 
                Vector3.Distance(_enemyTransform.position, _tower.transform.position) > _tower.Attributes.Range)
            {
                return typeof(RadarState);
            }

            UpdateRotation();
            
            if (_attackElapsedTime > _tower.Attributes.AttackSpeed)
            {
                _attackElapsedTime = 0;
                _ammoPool.Shoot(_enemyTransform.position);
            }

            _attackElapsedTime += Time.deltaTime;

            return typeof(AttackState);
        }

        public override void Exit()
        {}
        
        private void UpdateRotation()
        {
            var direction = _enemyTransform.position - _tower.transform.position;
            float step = _tower.Attributes.RotationSpeed * Time.deltaTime;

            Vector3 newDirection = Vector3.RotateTowards(_tower.RotatingElementTransform.forward, direction, step, 0.0f);

            _tower.RotatingElementTransform.rotation = Quaternion.LookRotation(newDirection);
        }
    }
}