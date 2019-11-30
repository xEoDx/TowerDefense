using System;
using System.Collections.Generic;
using AI;
using UnityEngine;

namespace Buildings.States
{
    public class RadarState : FSMState
    {
        private Tower _tower;
        private IList<Enemy> _enemies;
        public RadarState(Tower tower) : base(tower.gameObject)
        {
            _tower = tower;
        }

        public override void Init()
        {
            //TODO IMPORTANT: FETCH ENEMIES LIST FROM FUTURE EnemySpawner system
            _enemies = GameObject.FindObjectsOfType<Enemy>();
        }

        public override Type Execute()
        {
            UpdateRotation();
            
            foreach (var enemy in _enemies)
            {
                if (Vector3.Distance(enemy.transform.position, _tower.transform.position) < _tower.Attributes.Range)
                {
                    _tower.SetTarget(enemy);
                    return typeof(AttackState);
                }
            }

            return typeof(RadarState);
        }

        public override void Exit()
        {}
        
        private void UpdateRotation()
        {
            float step = _tower.Attributes.RotationSpeed * Time.deltaTime;
            _tower.RotatingElementTransform.rotation = Quaternion.Lerp(_tower.RotatingElementTransform.rotation, Quaternion.identity, step);
        }
    }
}
