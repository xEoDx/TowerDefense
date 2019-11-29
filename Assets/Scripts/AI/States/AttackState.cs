using System;
using System.Collections;
using System.Collections.Generic;
using Constants;
using UnityEngine;

namespace AI.States
{
    public class AttackState : FSMState
    {
        private static readonly int Attack = Animator.StringToHash("attack");
        private EnemyController _enemyController;
        private AmmoPool _ammoPool;
        private Transform _transform;
        private Tower _currentTarget;
        private Animator _animator;

        private float _lastAttackTime;

        public AttackState(EnemyController enemyController) : base(enemyController.gameObject)
        {
            _ammoPool = gameObject.GetComponent<AmmoPool>();
            _transform = gameObject.transform.GetChild(0).transform;
            _animator = _transform.GetComponent<Animator>();
            _enemyController = enemyController;
            _lastAttackTime = 0;
        }

        public override void Init()
        {
            _animator.SetTrigger(Attack);
        }

        public override Type Execute()
        {
            if (_enemyController.IsDead())
            {
                return typeof(DieState);
            }
            
            _currentTarget = GetClosestTower();

            if (_currentTarget != null)
            {
                if (_lastAttackTime >= _enemyController.AttackSpeed)
                {
                    _ammoPool.Shoot(_currentTarget.transform.position);
                    _lastAttackTime = 0;
                }

                _lastAttackTime += Time.deltaTime;
                
                return typeof(AttackState);
            }

            return typeof(RunState);

        }

        public override void Exit()
        {}
        
        Tower GetClosestTower()
        {
            int layerMask = Layers.Tower;
            Debug.DrawRay(_transform.position, _transform.forward * EnemyController.RayDistance, Color.red);

            RaycastHit hit;        
            if (Physics.Raycast(_transform.position, _transform.forward, out hit, EnemyController.RayDistance, layerMask))
            {
                var tower = hit.transform.GetComponent<Tower>();

                return tower;
            }

            return null;
        } 
    }
}