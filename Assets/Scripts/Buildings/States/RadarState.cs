using System;
using System.Collections.Generic;
using AI;
using FSM;
using Gameplay;
using UnityEngine;

namespace Buildings.States
{
    public class RadarState : FsmState
    {
        private readonly CanonTower _canonTower;
        
        public RadarState(CanonTower canonTower) : base(canonTower)
        {
            _canonTower = canonTower;
        }

        public override void Init()
        {}

        public override Type Execute()
        {
            UpdateRotation();

            Enemy closestEnemy = null;
            float closestEnemyDistance = float.MaxValue;
            foreach (var enemy in _canonTower.GetActiveEnemies())
            {
                var distance = Vector3.Distance(enemy.transform.position, _canonTower.transform.position);
                if (distance < _canonTower.TowerEntityAttributes.OffensiveAttributesData.Range)
                {
                    if (!_canonTower.IsBlockedByObstacle(enemy.transform.position) && distance < closestEnemyDistance)
                    {
                        closestEnemyDistance = distance;
                        closestEnemy = enemy;
                    }
                }
            }

            if (closestEnemy != null)
            {
                _canonTower.SetTarget(closestEnemy);
                return typeof(AttackState);
            }

            return typeof(RadarState);
        }

        public override void Exit()
        {}
        
        private void UpdateRotation()
        {
            float step = _canonTower.TowerEntityAttributes.MovementAttributesData.RotationSpeed * Time.deltaTime;
            _canonTower.RotatingElementTransform.rotation = Quaternion.Lerp(_canonTower.RotatingElementTransform.rotation, Quaternion.identity, step);
        }
    }
}
