using System;
using Constants;
using FSM;
using UnityEngine;
using UnityEngine.AI;

namespace AI.States
{
    public class RunState : FsmState
    {
        private static readonly float StuckTimeThreshold = 5.0f;
        private static readonly int Run = Animator.StringToHash("run");

        private readonly BasicEnemy _basicEnemy;
        private readonly Animator _animator;
        private readonly NavMeshAgent _agent;
        private readonly Transform _transform;
        private readonly Transform _playerBaseTransform;
        private Vector3 _currentDestinationPosition;
        private float _stuckElapsedTime;

        public RunState(BasicEnemy basicEnemy, Transform playerBaseTransform) : base(basicEnemy)
        {
            _transform = component.transform;
            _animator = _transform.GetChild(0).GetComponent<Animator>();
            _agent = _transform.GetComponent<NavMeshAgent>();
            _basicEnemy = basicEnemy;
            _playerBaseTransform = playerBaseTransform;
        }

        public override void Init()
        {
            _animator.SetTrigger(Run);
            _currentDestinationPosition = _playerBaseTransform.position;

            NavMeshHit hit;
            if (NavMesh.SamplePosition(_currentDestinationPosition, out hit, BasicEnemy.RayDistance * 5f, NavMesh.AllAreas))
            {
                _currentDestinationPosition = hit.position;
            }

            _agent.SetDestination(_currentDestinationPosition);
        }

        public override Type Execute()
        {
            if (_basicEnemy.IsDead())
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

            var distanceToTarget = Vector3.Distance(_transform.position, _currentDestinationPosition);

            if (distanceToTarget < 2)
            {
                return typeof(DieState);
            }

            return typeof(RunState);
        }

        public override void Exit()
        {
        }

        private void UpdateRotation()
        {
            var direction = _currentDestinationPosition - _transform.position;
            float step = _basicEnemy.EntityAttributes.MovementAttributesData.RotationSpeed * Time.deltaTime;

            Vector3 newDirection = Vector3.RotateTowards(_transform.forward, direction, step, 0.0f);

            _transform.rotation = Quaternion.LookRotation(newDirection);
        }

        private bool IsCollidingWithTower()
        {
            int layerMask = 1 << 9;
            Debug.DrawRay(_transform.position, _transform.forward * BasicEnemy.RayDistance, Color.white);

            RaycastHit hit;        
            if (Physics.Raycast(_transform.position, _transform.forward, out hit, BasicEnemy.RayDistance, layerMask))
            {
                return true;
            }

            return false;

        }
    }
}