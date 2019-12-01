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
    [RequireComponent(typeof(StateMachine))]
    [RequireComponent(typeof(AmmoPool))]
    public abstract class Tower : MonoBehaviour
    {
        #region Serializable Fields

        [SerializeField] private Transform rotatingElementTransform;
        public Transform RotatingElementTransform => rotatingElementTransform;

        [SerializeField] private bool forceToPlacedState = false;

        [SerializeField] private TowerAttributes towerAttributes;

        #endregion

        #region Properties

        public TowerAttributes TowerAttributes => towerAttributes;

        public AmmoPool AmmoPool { get; private set; }
        public bool IsPlaced { get; private set; }

        #endregion

        #region Fields

        protected float CurrentHealth;

        private StateMachine _stateMachine;
        private Enemy _currentTarget;
        private GameplayController _gameplayController;

        private int _enemyMask;
        private int _obstacleMask;

        #endregion

        #region Unity Event Functions

        void Awake()
        {
            AmmoPool = GetComponent<AmmoPool>();

            _currentTarget = null;
            CurrentHealth = TowerAttributes.DefensiveAttributesData.Health;
        }

        void Start()
        {
            _enemyMask = LayerMask.GetMask("Enemy");
            _obstacleMask = LayerMask.GetMask("Obstacle");

            _gameplayController = FindObjectOfType<GameplayController>();
            AmmoPool.InitAmmoPool(TowerAttributes.OffensiveAttributesData.Damage,
                TowerAttributes.OffensiveAttributesData.ProjectileSpeed);

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

        #endregion

        #region Abstract Interface

        public abstract void ReceiveDamage(float amount);

        #endregion

        #region Methods

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

            IsPlaced = true;
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

        public bool IsBlockedByObstacle(Vector3 enemyPosition)
        {
            var towerPosition = rotatingElementTransform.position;
            var direction = enemyPosition - towerPosition;
            var hits = Physics.RaycastAll(towerPosition,
                direction,
                TowerAttributes.OffensiveAttributesData.Range,
                _enemyMask | _obstacleMask);

            var enemyDistance = float.MaxValue;
            var obstacleDistance = float.MaxValue;
            foreach (var hit in hits)
            {
                var distance = Vector3.Distance(hit.transform.position, rotatingElementTransform.transform.position);
                if (1 << hit.transform.gameObject.layer == _enemyMask)
                {
                    if (distance < enemyDistance)
                    {
                        enemyDistance = distance;
                    }
                }
                else if (1 << hit.transform.gameObject.layer == _obstacleMask)
                {
                    if (distance < obstacleDistance)
                    {
                        obstacleDistance = distance;
                    }
                }
            }

            return obstacleDistance < enemyDistance;
        }

        #endregion
    }
}