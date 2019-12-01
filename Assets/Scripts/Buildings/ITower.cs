using AI;
using Ammo;
using Constants;
using UnityEngine;

namespace Buildings
{
    public interface ITower : IEntity
    {
        AmmoPool AmmoPool { get; }
        bool IsPlaced { get; }
        Transform GetTransform { get; }

        void Attack(Vector3 targetPosition);
        void ReceiveDamage(float amount);
        void DestroyTower();

    }
}