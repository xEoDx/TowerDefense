using System;
using FSM;
using UnityEngine;
using UnityEngine.AI;

namespace AI.States
{
    public class IdleState : FsmState
    {
        private readonly NavMeshAgent _agent;
        private readonly Enemy _enemy;
        private float _elapsedTime;
        private float _totalTime;

        public IdleState(Enemy enemy) : base(enemy)
        {
            _agent = enemy.transform.GetComponent<NavMeshAgent>();
            _enemy = enemy;
            _elapsedTime = 0;
            _totalTime = 0;
        }

        public override void Init()
        {
            _agent.speed = _enemy.EntityAttributesData.MovementAttributesData.MovementSpeed;
            
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