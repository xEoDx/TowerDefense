using UnityEngine;

namespace Ammo
{
    public interface IProjectile
    {
        float LifeTime { get; }        
        float Damage { get; }
        float Speed { get; }
        bool IsReady { get; }
        
        void Init(float damage, float attackRate);
        void Shoot(Vector3 position);
        void Reset();
    }
}