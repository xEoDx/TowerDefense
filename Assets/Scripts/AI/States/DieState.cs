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
        
        private readonly BasicEnemy _basicEnemy;
        private readonly Animator _animator;

        public DieState(BasicEnemy basicEnemy) : base(basicEnemy)
        {
            _animator = basicEnemy.transform.GetChild(0).GetComponent<Animator>();
            _basicEnemy = basicEnemy;
        }

        public override void Init()
        {
            // TODO play some particles
            _animator.SetTrigger(Die);
            _basicEnemy.gameObject.SetActive(false);
            
        }

        public override Type Execute()
        {
            return typeof(IdleState);
        }

        public override void Exit()
        {}
    }
}