using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AI.States
{
    public class DieState : FSMState
    {
        private static readonly int Die = Animator.StringToHash("die");
        
        private Enemy _enemy;
        private Animator _animator;

        public DieState(Enemy enemy) : base(enemy.gameObject)
        {
            _animator = enemy.transform.GetChild(0).GetComponent<Animator>();
            _enemy = enemy;
        }

        public override void Init()
        {
            // TODO play some particles
            _animator.SetTrigger(Die);
            _enemy.gameObject.SetActive(false);
            
        }

        public override Type Execute()
        {
            return typeof(IdleState);
        }

        public override void Exit()
        {}
    }
}