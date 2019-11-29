using UnityEngine;

namespace Ammo
{
    public abstract class Projectile : MonoBehaviour
    {
        protected readonly float LifeTime = 5.0f;
        
        [SerializeField] protected float damage;
        [SerializeField] protected float projectileSpeed;
                
        protected Vector3 Target;
        protected float ElapsedLifeTime;
        protected bool isReady;
        public bool IsReady => isReady;


        public void Init(float damage, float projectileSpeed)
        {
            this.damage = damage;
            this.projectileSpeed = projectileSpeed;
        }
       
        public abstract void Shoot(Vector3 position);
    }
}