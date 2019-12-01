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
    public class CanonTower : MonoBehaviour, ITower
    {
        #region Serializable Fields

        [SerializeField] private Transform rotatingElementTransform;
        public Transform RotatingElementTransform => rotatingElementTransform;

        [SerializeField] private bool forceToPlacedState;

        #endregion

        #region Properties
        public EntityAttributes EntityAttributes { get; private set; }
        public AmmoPool AmmoPool { get; private set; }
        public TowerType TowerType { get; private set; }
        public bool IsPlaced { get; private set; }
        public int BuildCost { get; private set; }
        public Transform GetTransform => transform;

        #endregion

        #region Fields
        
        private GameplayController _gameplayController;
        private StateMachine _stateMachine;
        private PlayerData _playerData;
        private BasicEnemy _currentTarget;
        private float _currentHealth;

        private int _enemyMask;
        private int _obstacleMask;

        #endregion

        #region Unity Event Functions

        private void Awake()
        {
            TowerType = TowerType.Canon;
            AmmoPool = GetComponent<AmmoPool>();
            _gameplayController = FindObjectOfType<GameplayController>();
            _playerData = FindObjectOfType<PlayerData>();
        }

        private void Start()
        {
            _currentTarget = null;
            _enemyMask = LayerMask.GetMask("Enemy");
            _obstacleMask = LayerMask.GetMask("Obstacle");

            BuildCost = _playerData.CanonBuildCost;
            EntityAttributes = _playerData.CanonEntityAttributes;
            _currentHealth = EntityAttributes.DefensiveAttributesData.Health;

            AmmoPool.InitAmmoPool(EntityAttributes.OffensiveAttributesData.Damage,
                EntityAttributes.OffensiveAttributesData.ProjectileSpeed);

            _stateMachine = GetComponent<StateMachine>();
            var towerFsmStates = new Dictionary<Type, FsmState>
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
            if (!HasHealth())
            {
                DestroyEntity();
            }
        }

        #endregion

        #region Interface Methods

        public void ReceiveDamage(float amount)
        {
            var updatedHealth = Mathf.Max(0, _currentHealth - amount);
            UpdateHealth(updatedHealth);
        }

        public void Attack(Vector3 targetPosition)
        {
            AmmoPool.Shoot(targetPosition);
        }

        public void DestroyEntity()
        {
            // TODO Play particles, etc...
            gameObject.SetActive(false);
        }

        #endregion

        #region Methods

        //TODO Move Render logic to a ITowerRenderer interface


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

        public BasicEnemy GetCurrentTarget()
        {
            return _currentTarget;
        }

        public void SetTarget(BasicEnemy basicEnemy)
        {
            _currentTarget = basicEnemy;
        }

        public void PlaceTower()
        {
            var renderers = transform.GetComponentsInChildren<MeshRenderer>();

            foreach (var meshRenderer in renderers)
            {
                var meshRendererMaterial = meshRenderer.material;
                meshRendererMaterial.color = Color.white;

                meshRenderer.material = meshRendererMaterial;
            }

            IsPlaced = true;
        }

        public bool IsBlockedByObstacle(Vector3 enemyPosition)
        {
            var towerPosition = rotatingElementTransform.position;
            var direction = enemyPosition - towerPosition;
            var hits = Physics.RaycastAll(towerPosition,
                direction,
                EntityAttributes.OffensiveAttributesData.Range,
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

        private bool HasHealth()
        {
            return _currentHealth > 0;
        }

        public IList<BasicEnemy> GetActiveEnemies()
        {
            return _gameplayController.GetActiveEnemies();
        }

        private void UpdateHealth(float newHealth)
        {
            _currentHealth = newHealth;
        }

        #endregion
    }
}