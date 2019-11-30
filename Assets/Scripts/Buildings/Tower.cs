using System;
using System.Collections.Generic;
using AI;
using Ammo;
using Buildings.States;
using FSM;
using Gameplay;
using UnityEngine;

namespace Buildings
{
    [RequireComponent(typeof(TowerAttributes))]
    [RequireComponent(typeof(StateMachine))]
    [RequireComponent(typeof(AmmoPool))]
    public abstract class Tower : MonoBehaviour
    {
        [SerializeField]
        private Transform rotatingElementTransform;

        [SerializeField] private bool forceToPlacedState = false;
        
        public Transform RotatingElementTransform => rotatingElementTransform;

        private TowerAttributes _attributes;
        public TowerAttributes Attributes => _attributes;

        
        private AmmoPool _ammoPool;
        public AmmoPool AmmoPool => _ammoPool;

        private bool _isPlaced;
        public bool IsPlaced => _isPlaced;

        protected float CurrentHealth;

        private StateMachine _stateMachine;
        private Enemy _currentTarget;
        private GameplayController _gameplayController;
      
        
        void Awake()
        {
            _attributes = GetComponent<TowerAttributes>();
            _ammoPool = GetComponent<AmmoPool>();
            
            _currentTarget = null;
            CurrentHealth = _attributes.Health;
        }

        void Start()
        {
            _gameplayController = FindObjectOfType<GameplayController>();
            _ammoPool.InitAmmoPool(_attributes.Damage, _attributes.ProjectileSpeed);
            
            _stateMachine = GetComponent<StateMachine>();
            var towerFsmStates = new Dictionary<Type, FSMState>
            {
                {typeof(PlacingState), new PlacingState(this)},
                {typeof(RadarState), new RadarState(this)},
                {typeof(AttackState), new AttackState(this)}
            };
            
            // Initial towers placed in the map
            Type initialState = null;
            if (forceToPlacedState)
            {
                PlaceTower();
                initialState = typeof(RadarState);
            }
            
            _stateMachine.SetStates(towerFsmStates, initialState);
        }

        private void Update()
        {
            if (IsDestroyed())
            {
                gameObject.SetActive(false);
            }
        }
        public abstract void ReceiveDamage(float amount);

        //TODO MOVE TO TOWERRENDERER
        public void SetUnplaceable()
        {
            var renderers = transform.GetComponentsInChildren<MeshRenderer>();

            foreach (var meshRenderer in renderers)
            {
                var meshRendererMaterial = meshRenderer.material;
                meshRendererMaterial.color = Color.red;

                meshRenderer.material = meshRendererMaterial;
            }
        }

        public void SetPlaceable()
        {
            var renderers = transform.GetComponentsInChildren<MeshRenderer>();

            foreach (var meshRenderer in renderers)
            {
                var meshRendererMaterial = meshRenderer.material;
                meshRendererMaterial.color = Color.green;

                meshRenderer.material = meshRendererMaterial;
            }
        }

        public Enemy GetCurrentTarget()
        {
            return _currentTarget;
        }

        public void SetTarget(Enemy enemy)
        {
            _currentTarget = enemy;
        }

        public void PlaceTower()
        {
            var renderers = transform.GetComponentsInChildren<MeshRenderer>();

            foreach (var meshRenderer in renderers)
            {
                var meshRendererMaterial = meshRenderer.material;
                meshRendererMaterial.color = Color.blue;

                meshRenderer.material = meshRendererMaterial;
            }
            _isPlaced = true;
        }
        
        private bool IsDestroyed()
        {
            return CurrentHealth <= 0;
        }

        public void UpdateHealth(float newHealth)
        {
            CurrentHealth = newHealth;
        }

        public IList<Enemy> GetActiveEnemies()
        {
            return _gameplayController.GetActiveEnemies();
        }
    }
}

