using Ammo;
using Buildings;
using UnityEngine;

namespace Entities
{
    public interface IEntity
    {
        EntityAttributes EntityAttributes { get; }
        
        AmmoPool AmmoPool { get; }
        
        Transform GetTransform { get; }

        void Attack(Vector3 targetPosition);
        void ReceiveDamage(float amount);
        void DestroyEntity();
    }
}