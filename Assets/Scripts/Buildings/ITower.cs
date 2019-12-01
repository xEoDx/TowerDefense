using Entities;

namespace Buildings
{
    public enum TowerType
    {
        Canon
      
    }

    public interface ITower : IEntity
    {
        TowerType TowerType { get; }
        bool IsPlaced { get; }
        int BuildCost { get; }
    }
}