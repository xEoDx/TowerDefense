using System;
using System.Collections;
using System.Collections.Generic;
using AI.States;
using Ammo;
using Constants;
using FSM;
using UnityEngine;

namespace AI
{
    [RequireComponent(typeof(StateMachine))]
    [RequireComponent(typeof(AmmoPool))]
    public class Enemy : MonoBehaviour
    {
        public static readonly float RayDistance = 2.0f;

        [field: Header("Movement values")]
        public float MovementSpeed { get; } = 8;
        public float RotationSpeed { get; } = 15;

        [Header("Offensive values")]
        [SerializeField] 
        private float damage = 20F;
        [SerializeField] 
        private float projectileSpeed = 250;

        public float AttackSpeed { get; } = 2.6F;

        [Header("Defensive values")]
        [SerializeField] private float health = 100;

        public float Health => health;

        private AmmoPool _ammoPool;

        private StateMachine _stateMachine;
        private float _currentHealth;

        private void Start()
        {
            _stateMachine = GetComponent<StateMachine>();
            _ammoPool = GetComponent<AmmoPool>();
            _ammoPool.InitAmmoPool(damage, projectileSpeed);
            _currentHealth = health;
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

        public void ReceiveDamage(float amount)
        {
            var newHealthvalue = Mathf.Max(0, _currentHealth - amount);
            _currentHealth = newHealthvalue;
        }

        public bool IsDead()
        {
            return _currentHealth <= 0;
        }

        public void Reset()
        {
            _currentHealth = health;
        }
    }
}