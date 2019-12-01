using AI;
using Buildings;
using Constants;
using UnityEngine;

namespace Ammo
{
    public class BulletProjectile : MonoBehaviour, IProjectile
    {
        public float LifeTime { get; private set; }
        public float Damage { get; private set; }
        public float Speed { get; private set; }
        public bool IsReady { get; private set; }

        private Vector3 _target;
        private float _elapsedLifeTime;

        private MeshRenderer _meshRenderer;

        public void Init(float damage, float attackRate)
        {
            Damage = damage;
            Speed = attackRate;
        }

        public void Awake()
        {
            _meshRenderer = GetComponent<MeshRenderer>();
            LifeTime = 2f;
            Reset();
        }

        private void Update()
        {
            if (!IsReady)
            {
                if (_elapsedLifeTime > LifeTime)
                {
                    Reset();
                }

                _elapsedLifeTime += Time.deltaTime;
                
                transform.position = Vector3.MoveTowards(transform.position, _target, Speed * Time.deltaTime);
            }
        }

        public void Shoot(Vector3 position)
        {
            IsReady = false;
            _target = position;
            transform.localPosition = Vector3.zero;
            _meshRenderer.enabled = true;
        }
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag(Tags.Tower) && transform.CompareTag(Tags.EnemyAmmo))
            {
                var tower = other.GetComponent<ITower>();
                tower.ReceiveDamage(Damage);
                Reset();
            }
            else if (other.CompareTag(Tags.Enemy) && transform.CompareTag(Tags.PlayerAmmo))
            {
                var enemy = other.GetComponent<BasicEnemy>();
                enemy.ReceiveDamage(Damage);
                Reset();
            }
        }

        public void Reset()
        {
            IsReady = true;
            _meshRenderer.enabled = false;
            transform.localPosition = Vector3.zero;
            _elapsedLifeTime = 0;
        }
    }
}
