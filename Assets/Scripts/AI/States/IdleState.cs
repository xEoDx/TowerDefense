using System;
using FSM;
using UnityEngine;
using UnityEngine.AI;

namespace AI.States
{
    public class IdleState : FsmState
    {
        private readonly NavMeshAgent _agent;
        private readonly BasicEnemy _basicEnemy;
        private float _elapsedTime;
        private float _totalTime;

        public IdleState(BasicEnemy basicEnemy) : base(basicEnemy)
        {
            _agent = basicEnemy.transform.GetComponent<NavMeshAgent>();
            _basicEnemy = basicEnemy;
            _elapsedTime = 0;
            _totalTime = 0;
        }

        public override void Init()
        {
            _agent.speed = _basicEnemy.EntityAttributes.MovementAttributesData.MovementSpeed;
            
            _totalTime = UnityEngine.Random.Range(0, 1f);
        }

        public override Type Execute()
        {
            if (_elapsedTime > _totalTime)
            {
                return typeof(RunState);
            }
            _elapsedTime += Time.deltaTime;

            return typeof(IdleState);
        }

        public override void Exit()
        {
        }
    }
}