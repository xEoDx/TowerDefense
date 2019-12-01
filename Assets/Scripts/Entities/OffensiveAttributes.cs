using System;
using UnityEngine;

namespace Entities
{
    [Serializable]
    public struct OffensiveAttributes
    {
        [Header("Offensive attributes")] 
        [SerializeField]
        private float range;
        public float Range => range;

        [SerializeField] 
        private float damage;
        public float Damage => damage;

        [SerializeField] 
        private float attackSpeed;
        public float AttackSpeed => attackSpeed;

        [SerializeField] 
        private float projectileSpeed;
        public float ProjectileSpeed => projectileSpeed;
    }
}