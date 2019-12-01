using System;
using UnityEngine;

namespace Entities
{
    [Serializable]
    public struct DefensiveAttributes
    {
        [Header("Defensive Attributes")] 
        [SerializeField]
        private int health;
        public int Health => health;
        
        //TODO: armor, elemental resist...
    }
}
