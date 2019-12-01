using System.Collections.Generic;
using System.Linq;
using Constants;
using UnityEngine;

namespace Ammo
{
    public class AmmoPool : MonoBehaviour
    {
        private enum ProjectileOwner
        {
            Player,
            Enemy
        }

        [SerializeField] private ProjectileOwner owner;
        [SerializeField] private GameObject projectilePrefab;
        [SerializeField] private Transform projectileParentTransform;
        [SerializeField] private int poolSize = 10;

        private IList<IProjectile> _projectiles;

        private void Awake()
        {
            _projectiles = new List<IProjectile>(poolSize);
        }

        public void InitAmmoPool(float projectileDamage, float projectileSpeed)
        {
            for (int i = 0; i < poolSize; i++)
            {
                var projectileObject = Instantiate(projectilePrefab, projectileParentTransform);
                switch (owner)
                {
                    case ProjectileOwner.Player:
                        projectileObject.tag = Tags.PlayerAmmo;
                        break;
                    case ProjectileOwner.Enemy:
                        projectileObject.tag = Tags.EnemyAmmo;
                        break;
                    default:
                        Debug.LogError("Owner for AmmoPool not set up in GameObject: "+transform.name);
                        break;
                }
                var projectile = projectileObject.GetComponent<IProjectile>();
                projectile.Init(projectileDamage, projectileSpeed);
                _projectiles.Add(projectile);
            }
        }

        public void Shoot(Vector3 position)
        {
            var projectile = _projectiles.First(p => p.IsReady);

            projectile?.Shoot(position);
        }
    }
}
