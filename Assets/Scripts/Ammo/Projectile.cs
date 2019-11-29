using UnityEngine;

namespace Ammo
{
    public abstract class Projectile : MonoBehaviour
    {
        protected readonly float LifeTime = 5.0f;
        
        [SerializeField] protected float damage;
        [SerializeField] protected float projectileSpeed;
                
        protected Vector3 target;
        protected float elapsedLifeTime;
        protected bool isReady;
        public bool IsReady => isReady;

        
        public abstract void Shoot(Vector3 position);
    }
}