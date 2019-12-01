using Buildings;
using Entities;
using UnityEngine;

namespace Gameplay
{
    public class PlayerData : MonoBehaviour
    {
        [SerializeField] [Tooltip("Amount of collisions on the base so that game is over")]
        private int playerBaseHealth = 3;

        public int PlayerBaseHealth => playerBaseHealth;

        // TODO this should be done from a TowerManager which controls the stats (based on tower level for instance)
        [Header("Canon Tower")] [SerializeField]
        private EntityAttributes canonEntityAttributes;

        public EntityAttributes CanonEntityAttributes => canonEntityAttributes;

        [SerializeField] private int canonBuildCost;
        public int CanonBuildCost => canonBuildCost;


        public int GetTowerCost(TowerType type)
        {
            int cost = 0;
            switch (type)
            {
                case TowerType.Canon:
                    cost = canonBuildCost;
                    break;
            }

            return cost;
        }

        public EntityAttributes GetTowerAttributes(TowerType type)
        {
            EntityAttributes attributes = new EntityAttributes();

            switch (type)
            {
                case TowerType.Canon:
                    attributes = canonEntityAttributes;
                    break;
            }

            return attributes;
        }
    }
}