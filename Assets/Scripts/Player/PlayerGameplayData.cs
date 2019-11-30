using System;
using UnityEngine;

namespace Player
{
    [Serializable]
    public struct PlayerGameplayData
    {
        [SerializeField]
        private int income;

        public int Income => income;

        public PlayerGameplayData(int income)
        {
            this.income = income;
        }

        public void AddIncome(int amount)
        {
            income += amount;
        }

        public void SubtractIncome(int amount)
        {
            income = Mathf.Max(0, income - amount);
        }
    }
}