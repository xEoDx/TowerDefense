
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
        EnemyType EnemyType { get; }
        int Reward { get; }


        void InitEnemy();


    }
}