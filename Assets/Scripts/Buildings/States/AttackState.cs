using System;
using AI;
using Ammo;
using FSM;
using UnityEngine;

namespace Buildings.States
{
    public class AttackState : FsmState
    {
        private const float MaxStuckTime = 1.5f;

        private readonly CanonTower _canonTower;
        private readonly AmmoPool _ammoPool;
        private BasicEnemy _currentTarget;
        private Transform _enemyTransform;
        private float _attackElapsedTime;
        private float _elapsedStuckTime;

        public AttackState(CanonTower canonTower) : base(canonTower)
        {
            _canonTower = canonTower;
            _ammoPool = _canonTower.AmmoPool;
        }

        public override void Init()
        {
            _elapsedStuckTime = 0;
            _attackElapsedTime = 0;
            _currentTarget = _canonTower.GetCurrentTarget();
            _enemyTransform = _canonTower.GetCurrentTarget().transform;
        }

        public override Type Execute()
        {
            if (_currentTarget.IsDead() ||
                Vector3.Distance(_enemyTransform.position, _canonTower.transform.position) >
                _canonTower.EntityAttributes.OffensiveAttributesData.Range)
            {
                return typeof(RadarState);
            }

            UpdateRotation();

            if (IsFacingEnemy(_enemyTransform.position))
            {
                if (!_canonTower.IsBlockedByObstacle(_enemyTransform.position))
                {
                    _elapsedStuckTime = 0;
                    if (_attackElapsedTime > _canonTower.EntityAttributes.OffensiveAttributesData.AttackSpeed)
                    {
                        _attackElapsedTime = 0;
                        _canonTower.Attack(_enemyTransform.position);
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
            var heading = (enemyPosition - _canonTower.RotatingElementTransform.position).normalized;
            var dotProduct = Vector3.Dot(heading, _canonTower.RotatingElementTransform.forward);

            return dotProduct > 0.85f;
        }


        public override void Exit()
        {
        }

        private void UpdateRotation()
        {
            var direction = _enemyTransform.position - _canonTower.transform.position;
            float step = _canonTower.EntityAttributes.MovementAttributesData.RotationSpeed * Time.deltaTime;

            Vector3 newDirection =
                Vector3.RotateTowards(_canonTower.RotatingElementTransform.forward, direction, step, 0.0f);

            _canonTower.RotatingElementTransform.rotation = Quaternion.LookRotation(newDirection);
        }
    }
}