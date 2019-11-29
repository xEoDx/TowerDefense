using System;
using UnityEngine;
using UnityEngine.AI;

namespace AI.States
{
    public class IdleState : FSMState
    {
        private NavMeshAgent _agent;
        private Enemy _enemy;
        private float _elapsedTime;
        private float _totalTime;

        public IdleState(Enemy enemy) : base(enemy.gameObject)
        {
            _agent = gameObject.transform.GetChild(0).GetComponent<NavMeshAgent>();
            _enemy = enemy;
            _elapsedTime = 0;
            _totalTime = 0;
        }

        public override void Init()
        {
            _agent.speed = _enemy.MovementSpeed;
            
            _totalTime = UnityEngine.Random.Range(0, 1.5f);
            Debug.Log("[IdleState] Init, rnd: "+_totalTime);
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
            Debug.Log("[IdleState] Exit");
        }
    }
}