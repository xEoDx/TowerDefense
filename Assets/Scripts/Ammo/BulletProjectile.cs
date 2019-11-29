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
                if (elapsedLifeTime > LifeTime)
                {
                    ResetProjectile();
                }

                elapsedLifeTime += Time.deltaTime;
                
                transform.position = Vector3.MoveTowards(transform.position, target, projectileSpeed * Time.deltaTime);
            }
        }

        public override void Shoot(Vector3 position)
        {
            isReady = false;
            target = position;
            transform.localPosition = Vector3.zero;
            _meshRenderer.enabled = true;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag(Tags.Tower))
            {
                var tower = other.GetComponent<Tower>();
                tower.ReceiveDamage(damage);
                ResetProjectile();
            }
        }

        private void ResetProjectile()
        {
            isReady = true;
            _meshRenderer.enabled = false;
            transform.localPosition = Vector3.zero;
            elapsedLifeTime = 0;
        }
    }
}
