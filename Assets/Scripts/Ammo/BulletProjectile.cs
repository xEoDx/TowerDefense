using AI;
using Buildings;
using Constants;
using UnityEngine;

namespace Ammo
{
    public class BulletProjectile : Projectile
    {
        private MeshRenderer _meshRenderer;

        public void Awake()
        {
            _meshRenderer = GetComponent<MeshRenderer>();
            ResetProjectile();
        }

        private void Update()
        {
            if (!isReady)
            {
                if (ElapsedLifeTime > LifeTime)
                {
                    ResetProjectile();
                }

                ElapsedLifeTime += Time.deltaTime;
                
                transform.position = Vector3.MoveTowards(transform.position, Target, projectileSpeed * Time.deltaTime);
            }
        }

        public override void Shoot(Vector3 position)
        {
            isReady = false;
            Target = position;
            transform.localPosition = Vector3.zero;
            _meshRenderer.enabled = true;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag(Tags.Tower) && transform.CompareTag(Tags.EnemyAmmo))
            {
                var tower = other.GetComponent<ITower>();
                tower.ReceiveDamage(damage);
                ResetProjectile();
            }
            else if (other.CompareTag(Tags.Enemy) && transform.CompareTag(Tags.PlayerAmmo))
            {
                var enemy = other.GetComponent<BasicEnemy>();
                enemy.ReceiveDamage(damage);
                ResetProjectile();
            }
        }

        private void ResetProjectile()
        {
            isReady = true;
            _meshRenderer.enabled = false;
            transform.localPosition = Vector3.zero;
            ElapsedLifeTime = 0;
        }
    }
}
