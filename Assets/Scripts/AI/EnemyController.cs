using System;
using System.Collections;
using System.Collections.Generic;
using AI.States;
using Constants;
using FSM;
using UnityEngine;

namespace AI
{
    [RequireComponent(typeof(StateMachine))]
    [RequireComponent(typeof(AmmoPool))]
    public class EnemyController : MonoBehaviour
    {
        public static readonly float RayDistance = 2.0f;
        
        [Header("Movement values")]
        [SerializeField] private float movementSpeed = 8;
        public float MovementSpeed => movementSpeed;
        
        [SerializeField] private float rotationSpeed = 15;
        public float RotationSpeed => rotationSpeed;
        
        [Header("Offensive values")]
        [SerializeField] private float damage = 20F;
        public float Damage => damage;

        [SerializeField] private float attackSpeed = 2.6F;
        public float AttackSpeed => attackSpeed;

        [Header("Defensive values")]
        [SerializeField] private float health = 100;

        public float Health => health;


        private StateMachine _stateMachine;

        private void Start()
        {
            _stateMachine = GetComponent<StateMachine>();
            var playerBaseTransform = GameObject.FindWithTag(Tags.PlayerBase).transform;

            var states = new Dictionary<Type, FSMState>
            {
                {typeof(IdleState), new IdleState(this)},
                {typeof(RunState), new RunState(this, playerBaseTransform)},
                {typeof(AttackState), new AttackState(this)},
                {typeof(DieState), new DieState(this)}
            };

            _stateMachine.SetStates(states);
        }

        public bool IsDead()
        {
            return health <= 0;
        }
    }
}