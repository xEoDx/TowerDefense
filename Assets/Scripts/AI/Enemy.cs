using System;
using System.Collections;
using System.Collections.Generic;
using AI.States;
using Ammo;
using Buildings;
using Constants;
using Entities;
using FSM;
using UnityEngine;

namespace AI
{
    [RequireComponent(typeof(StateMachine))]
    [RequireComponent(typeof(AmmoPool))]
    public class Enemy : MonoBehaviour
    {
        public static readonly float RayDistance = 2.0f;

        [SerializeField] private EntityAttributes entityAttributes;
        public EntityAttributes EntityAttributesData => entityAttributes;

        [Header("Other")] 
        [SerializeField] 
        [Tooltip("Amount of income a player gets when killing this enemy")]
        private int reward = 50;

        public int Reward => reward;

        private float _currentHealth;
        private AmmoPool _ammoPool;

        private StateMachine _stateMachine;

        private void Start()
        {
            _stateMachine = GetComponent<StateMachine>();
            _ammoPool = GetComponent<AmmoPool>();
            _ammoPool.InitAmmoPool(entityAttributes.OffensiveAttributesData.Damage, entityAttributes.OffensiveAttributesData.ProjectileSpeed);
            _currentHealth = entityAttributes.DefensiveAttributesData.Health;
            var playerBaseTransform = GameObject.FindWithTag(Tags.PlayerBase).transform;

            var states = new Dictionary<Type, FsmState>
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
            _currentHealth = entityAttributes.DefensiveAttributesData.Health;
        }
    }
}