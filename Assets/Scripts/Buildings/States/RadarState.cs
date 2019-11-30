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
            
            foreach (var enemy in _tower.GetActiveEnemies())
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
