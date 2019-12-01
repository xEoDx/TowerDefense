﻿using System;
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
    public class BasicEnemy : MonoBehaviour, IEnemy
    {
        public static readonly float RayDistance = 2.0f;

        [SerializeField] private EntityAttributes enemyAttributes;
        public EntityAttributes EntityAttributes { get; }
        public AmmoPool AmmoPool { get; }
        public Transform GetTransform { get; }

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
            _ammoPool.InitAmmoPool(enemyAttributes.OffensiveAttributesData.Damage, enemyAttributes.OffensiveAttributesData.ProjectileSpeed);
            _currentHealth = enemyAttributes.DefensiveAttributesData.Health;
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

        public void Attack(Vector3 targetPosition)
        {
            _ammoPool.Shoot(targetPosition);
        }

        public void ReceiveDamage(float amount)
        {
            var newHealthvalue = Mathf.Max(0, _currentHealth - amount);
            _currentHealth = newHealthvalue;
        }

        public void DestroyEntity()
        {
            gameObject.SetActive(false);
        }

        public bool IsDead()
        {
            return _currentHealth <= 0;
        }

        public void Reset()
        {
            _currentHealth = enemyAttributes.DefensiveAttributesData.Health;
        }

        
    }
}