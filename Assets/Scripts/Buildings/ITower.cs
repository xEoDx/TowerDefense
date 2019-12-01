using AI;
using Ammo;
using UnityEngine;

namespace Buildings
{
    public interface ITower
    {
        EntityAttributes TowerEntityAttributes { get; }
        AmmoPool AmmoPool { get; }
        bool IsPlaced { get; }
        Transform GetTransform { get; }

        void Attack(Vector3 targetPosition);
        void ReceiveDamage(float amount);
        void DestroyTower();

    }
}