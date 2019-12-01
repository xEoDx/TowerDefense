using System;
using System.Collections;
using System.Collections.Generic;
using Ammo;
using Buildings;
using Constants;
using FSM;
using UnityEngine;

namespace AI.States
{
    public class AttackState : FsmState
    {
        private static readonly int Attack = Animator.StringToHash("attack");
        private readonly BasicEnemy _basicEnemy;
        private readonly AmmoPool _ammoPool;
        private readonly Transform _transform;
        private readonly Animator _animator;
        private ITower _currentTarget;

        private float _lastAttackTime;

        public AttackState(BasicEnemy basicEnemy) : base(basicEnemy)
        {
            _transform = basicEnemy.transform;
            _ammoPool = basicEnemy.GetComponent<AmmoPool>();
            _animator = _transform.GetChild(0).GetComponent<Animator>();
            _basicEnemy = basicEnemy;
            _lastAttackTime = 0;
        }

        public override void Init()
        {
            _animator.SetTrigger(Attack);
        }

        public override Type Execute()
        {
            if (_basicEnemy.IsDead())
            {
                return typeof(DieState);
            }
            
            _currentTarget = GetClosestTower();

            if (_currentTarget != null)
            {
                if (_lastAttackTime >= _basicEnemy.EntityAttributes.OffensiveAttributesData.AttackSpeed)
                {
                    _ammoPool.Shoot(_currentTarget.GetTransform.position);
                    _lastAttackTime = 0;
                }

                _lastAttackTime += Time.deltaTime;
                
                return typeof(AttackState);
            }

            return typeof(RunState);
        }

        public override void Exit()
        {}
        
        ITower GetClosestTower()
        {
            int layerMask = Layers.Tower;
            Debug.DrawRay(_transform.position, _transform.forward * BasicEnemy.RayDistance, Color.red);

            RaycastHit hit;        
            if (Physics.Raycast(_transform.position, _transform.forward, out hit, BasicEnemy.RayDistance, layerMask))
            {
                var tower = hit.transform.GetComponent<ITower>();

                return tower;
            }

            return null;
        } 
    }
}