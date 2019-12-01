
using Constants;
using Entities;

namespace AI
{
    public interface IEnemy : IEntity
    {
        int Reward { get; }

        
    }
}