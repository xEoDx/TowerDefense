using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AI.States
{
    public class DieState : FSMState
    {
        private static readonly int Die = Animator.StringToHash("die");
        
        private EnemyController _enemyController;
        private Animator _animator;

        public DieState(EnemyController enemyController) : base(enemyController.gameObject)
        {
            _animator = enemyController.transform.GetChild(0).GetComponent<Animator>();
            _enemyController = enemyController;
        }

        public override void Init()
        {
            // TODO play some particles
            _animator.SetTrigger(Die);
            _enemyController.gameObject.SetActive(false);
            
        }

        public override Type Execute()
        {
            return typeof(DieState);
        }

        public override void Exit()
        {}
    }
}