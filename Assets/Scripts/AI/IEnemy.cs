
using Constants;
using Entities;
using Gameplay;

namespace AI
{
    public enum EnemyType
    {
        Basic,
        Fast,
        Big
    }
    
    public interface IEnemy : IEntity
    {
        int Reward { get; }
        EnemyType EnemyType { get; }

        
    }
}