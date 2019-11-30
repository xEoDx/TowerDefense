using System;
using Constants;
using UnityEngine;
using UnityEngine.AI;

namespace AI.States
{
    public class RunState : FSMState
    {
        private static readonly float StuckTimeThreshold = 5.0f;
        private static readonly int Run = Animator.StringToHash("run");
        private static readonly int WalkableMask = 1 << NavMesh.GetAreaFromName("Walkable");

        private Enemy _enemy;
        private Animator _animator;
        private NavMeshAgent _agent;
        private Transform _transform;
        private Transform _playerBaseTransform;
        private Vector3 _currentDestinationPosition;
        private float _stuckElapsedTime;

        public RunState(Enemy enemy, Transform playerBaseTransform) : base(enemy
            .gameObject)
        {
            _transform = gameObject.transform;
            _animator = _transform.GetChild(0).GetComponent<Animator>();
            _agent = _transform.GetComponent<NavMeshAgent>();
            _enemy = enemy;
            _playerBaseTransform = playerBaseTransform;
        }

        public override void Init()
        {
            _animator.SetTrigger(Run);
            _currentDestinationPosition = _playerBaseTransform.position;

            NavMeshHit hit;
            if (NavMesh.SamplePosition(_currentDestinationPosition, out hit, Enemy.RayDistance * 5f, NavMesh.AllAreas))
            {
                _currentDestinationPosition = hit.position;
            }
            Debug.Log("[RunState] Running to target at position " + _currentDestinationPosition);

            _agent.SetDestination(_currentDestinationPosition);
        }

        public override Type Execute()
        {
            if (_enemy.IsDead())
            {
                return typeof(DieState);
            }
            
            UpdateRotation();

            if (IsCollidingWithTower())
            {
                _stuckElapsedTime += Time.deltaTime;

                if (_stuckElapsedTime > StuckTimeThreshold)
                {
                    return typeof(AttackState);
                }
            }
            else
            {
                _stuckElapsedTime = 0f;
            }
            
            return typeof(RunState);
        }

        public override void Exit()
        {
        }

        private void UpdateRotation()
        {
            var direction = _currentDestinationPosition - _transform.position;
            float step = _enemy.RotationSpeed * Time.deltaTime;

            Vector3 newDirection = Vector3.RotateTowards(_transform.forward, direction, step, 0.0f);

            _transform.rotation = Quaternion.LookRotation(newDirection);
        }

        private bool IsCollidingWithTower()
        {
            int layerMask = 1 << 9;
            Debug.DrawRay(_transform.position, _transform.forward * Enemy.RayDistance, Color.white);

            RaycastHit hit;        
            if (Physics.Raycast(_transform.position, _transform.forward, out hit, Enemy.RayDistance, layerMask))
            {
                return true;
            }

            return false;

        }
    }
}