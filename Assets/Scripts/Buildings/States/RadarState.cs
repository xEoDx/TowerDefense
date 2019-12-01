using System;
using System.Collections.Generic;
using AI;
using Gameplay;
using UnityEngine;

namespace Buildings.States
{
    public class RadarState : FSMState
    {
        private Tower _tower;
        
        public RadarState(Tower tower) : base(tower.gameObject)
        {
            _tower = tower;
        }

        public override void Init()
        {}

        public override Type Execute()
        {
            UpdateRotation();

            Enemy closestEnemy = null;
            float closestEnemyDistance = float.MaxValue;
            foreach (var enemy in _tower.GetActiveEnemies())
            {
                var distance = Vector3.Distance(enemy.transform.position, _tower.transform.position);
                if (distance < _tower.EntityAttributes.OffensiveAttributesData.Range)
                {
                    if (!_tower.IsBlockedByObstacle(enemy.transform.position) && distance < closestEnemyDistance)
                    {
                        closestEnemyDistance = distance;
                        closestEnemy = enemy;
                    }
                }
            }

            if (closestEnemy != null)
            {
                _tower.SetTarget(closestEnemy);
                return typeof(AttackState);
            }

            return typeof(RadarState);
        }

        public override void Exit()
        {}
        
        private void UpdateRotation()
        {
            float step = _tower.EntityAttributes.MovementAttributesData.RotationSpeed * Time.deltaTime;
            _tower.RotatingElementTransform.rotation = Quaternion.Lerp(_tower.RotatingElementTransform.rotation, Quaternion.identity, step);
        }
    }
}
