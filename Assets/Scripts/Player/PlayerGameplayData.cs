using System;
using UnityEngine;

namespace Player
{
    [Serializable]
    public struct PlayerGameplayData
    {
        public int Income { get; private set; }

        public PlayerGameplayData(int income)
        {
            Income = income;
        }

        public int AddIncome(int amount)
        {
            Income += amount;
            return Income;
        }

        public int SubtractIncome(int amount)
        {
            Income = Mathf.Max(0, Income - amount);
            return Income;
        }
    }
}