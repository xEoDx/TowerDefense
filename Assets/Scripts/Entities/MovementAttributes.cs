using System;
using UnityEngine;

namespace Entities
{
    [Serializable]
    public struct MovementAttributes
    {
        [Header("Movement Attributes")] 
        [SerializeField]
        private float movementSpeed;
        public float MovementSpeed => movementSpeed;
        
        [SerializeField] 
        private float rotationSpeed;
        public float RotationSpeed => rotationSpeed;
    }
}
