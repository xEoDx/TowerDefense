using System;
using System.Collections;
using System.Collections.Generic;
using FSM;
using UnityEngine;

namespace AI.States
{
    public class DieState : FsmState
    {
        private static readonly int Die = Animator.StringToHash("die");
        
        private readonly Enemy _enemy;
        private readonly Animator _animator;

        public DieState(Enemy enemy) : base(enemy)
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