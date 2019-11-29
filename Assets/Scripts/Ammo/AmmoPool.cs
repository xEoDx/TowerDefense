using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Ammo;
using UnityEngine;

public class AmmoPool : MonoBehaviour
{
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Transform projectileParentTransform;
    [SerializeField] private int poolSize = 10;

    private IList<Projectile> _projectiles;

    private void Awake()
    {
        _projectiles = new List<Projectile>(poolSize);
        for (int i = 0; i < poolSize; i++)
        {
            var projectileObject = Instantiate(projectilePrefab, projectileParentTransform);
            var projectile = projectileObject.GetComponent<Projectile>();
            _projectiles.Add(projectile);
        }
    }

    public void Shoot(Vector3 position)
    {
        var projectile = _projectiles.First(p => p.IsReady);

        projectile?.Shoot(position);
    }
}
