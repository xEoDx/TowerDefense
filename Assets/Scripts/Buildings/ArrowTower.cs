using UnityEngine;

namespace Buildings
{
    public class ArrowTower : Tower
    {
        public override void ReceiveDamage(float amount)
        {
            var updatedHealth = Mathf.Max(0, CurrentHealth - amount);
            UpdateHealth(updatedHealth);
        }
    }
}