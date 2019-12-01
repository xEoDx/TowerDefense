using System;
using AI;
using Ammo;
using UnityEngine;

namespace Buildings.States
{
    public class AttackState : FSMState
    {
        private const float MaxStuckTime = 1.5f;

        private Tower _tower;
        private AmmoPool _ammoPool;
        private Enemy _currentTarget;
        private Transform _enemyTransform;
        private float _attackElapsedTime;
        private float _elapsedStuckTime;

        public AttackState(Tower tower) : base(tower.gameObject)
        {
            _tower = tower;
            _ammoPool = _tower.AmmoPool;
        }

        public override void Init()
        {
            _elapsedStuckTime = 0;
            _attackElapsedTime = 0;
            _currentTarget = _tower.GetCurrentTarget();
            _enemyTransform = _tower.GetCurrentTarget().transform;
        }

        public override Type Execute()
        {
            if (_currentTarget.IsDead() ||
                Vector3.Distance(_enemyTransform.position, _tower.transform.position) >
                _tower.TowerAttributes.OffensiveAttributesData.Range)
            {
                return typeof(RadarState);
            }

            UpdateRotation();

            if (IsFacingEnemy(_enemyTransform.position))
            {
                if (!_tower.IsBlockedByObstacle(_enemyTransform.position))
                {
                    _elapsedStuckTime = 0;
                    if (_attackElapsedTime > _tower.TowerAttributes.OffensiveAttributesData.AttackSpeed)
                    {
                        _attackElapsedTime = 0;
                        _ammoPool.Shoot(_enemyTransform.position);
                    }

                    _attackElapsedTime += Time.deltaTime;
                }
                else
                {
                    if (_elapsedStuckTime > MaxStuckTime)
                    {
                        return typeof(RadarState);
                    }

                    _elapsedStuckTime += Time.deltaTime;
                }
            }

            return typeof(AttackState);
        }

        private bool IsFacingEnemy(Vector3 enemyPosition)
        {
            var heading = (enemyPosition - _tower.RotatingElementTransform.position).normalized;
            var dotProduct = Vector3.Dot(heading, _tower.RotatingElementTransform.forward);

            return dotProduct > 0.85f;
        }


        public override void Exit()
        {
        }

        private void UpdateRotation()
        {
            var direction = _enemyTransform.position - _tower.transform.position;
            float step = _tower.TowerAttributes.MovementAttributesData.RotationSpeed * Time.deltaTime;

            Vector3 newDirection =
                Vector3.RotateTowards(_tower.RotatingElementTransform.forward, direction, step, 0.0f);

            _tower.RotatingElementTransform.rotation = Quaternion.LookRotation(newDirection);
        }
    }
}