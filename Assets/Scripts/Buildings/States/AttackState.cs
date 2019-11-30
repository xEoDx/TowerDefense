using System;
using AI;
using Ammo;
using UnityEngine;

namespace Buildings.States
{
    public class AttackState : FSMState
    {
        private readonly int _enemyMask = LayerMask.GetMask("Enemy");
        private readonly int _obstacleMask = LayerMask.GetMask("Obstacle");
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
                Vector3.Distance(_enemyTransform.position, _tower.transform.position) > _tower.Attributes.Range)
            {
                return typeof(RadarState);
            }

            UpdateRotation();

            if (IsFacingEnemy(_enemyTransform.position))
            {
                if (!IsObstacleInFront())
                {
                    _elapsedStuckTime = 0;
                    if (_attackElapsedTime > _tower.Attributes.AttackSpeed)
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
        
        private bool IsObstacleInFront()
        {
            var direction = _enemyTransform.position - _tower.RotatingElementTransform.position;
            var hits = Physics.RaycastAll(_tower.RotatingElementTransform.position,
                direction,
                _tower.Attributes.Range,
                _enemyMask | _obstacleMask);
 
            float enemyDistance = float.MaxValue;
            float obstacleDistance = float.MaxValue;
            foreach (var hit in hits)
            {
                var distance = Vector3.Distance(hit.transform.position, _tower.RotatingElementTransform.transform.position);
                if (1 << hit.transform.gameObject.layer == _enemyMask )
                {
                    if (distance < enemyDistance)
                    {
                        enemyDistance = distance;
                    }
                }
                else if (1 << hit.transform.gameObject.layer == _obstacleMask)
                {
                    if (distance < obstacleDistance)
                    {
                        obstacleDistance = distance;
                    }
                }
            }

            return obstacleDistance < enemyDistance;
        }
        
        private bool IsFacingEnemy(Vector3 enemyPosition)
        {
            var heading = enemyPosition - _tower.RotatingElementTransform.position;
            
            var dotProduct = Vector3.Dot(heading, _tower.RotatingElementTransform.forward);

            

            return dotProduct < 15;
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