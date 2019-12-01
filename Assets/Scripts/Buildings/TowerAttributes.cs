using System;
using Entities;
using UnityEngine;

namespace Buildings
{
    [Serializable]
    public struct TowerAttributes
    {
        [SerializeField]
        private OffensiveAttributes offensiveAttributes;
        public OffensiveAttributes OffensiveAttributesData => offensiveAttributes;

        [SerializeField] 
        private DefensiveAttributes defensiveAttributes;
        public DefensiveAttributes DefensiveAttributesData => defensiveAttributes;

        [SerializeField] 
        private MovementAttributes movementAttributes;
        public MovementAttributes MovementAttributesData => movementAttributes;
    }
}