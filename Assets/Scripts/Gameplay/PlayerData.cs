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
        [Header("Canon Tower")] 
        [SerializeField] private EntityAttributes cannonEntityAttributes;
        public EntityAttributes CanonEntityAttributes => cannonEntityAttributes;
    }
}