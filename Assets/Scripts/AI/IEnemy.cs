
using Constants;

namespace AI
{
    public interface IEnemy : IEntity
    {
        int Reward { get; }

        void ReceiveDamage(float amount);
    }
}