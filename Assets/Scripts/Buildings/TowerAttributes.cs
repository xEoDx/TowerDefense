using UnityEngine;

namespace Buildings
{
    public class TowerAttributes : MonoBehaviour
    {
        [Header("Offensive attributes")] 
        [SerializeField]
        private float range = 5;
        public float Range => range;

        [SerializeField] 
        private float damage = 10;
        public float Damage => damage;

        [SerializeField] 
        private float attackSpeed = 1;
        public float AttackSpeed => attackSpeed;

        [SerializeField]
        private float rotationSpeed = 20;

        public float RotationSpeed => rotationSpeed;

        [SerializeField] 
        private float projectileSpeed = 50;
        public float ProjectileSpeed => projectileSpeed;
        
        [Header("Defensive attributes")]
        [SerializeField]
        private float health = 100;
        public float Health => health;

    }
}