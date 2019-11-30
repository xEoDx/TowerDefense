using System.Collections.Generic;

namespace Gameplay
{
    public struct Level
    {
        private IList<Wave> _waves;
        public IList<Wave> Waves => _waves;

        private int _initialIncome;
        public int InitialIncome => _initialIncome;

        public Level(IList<Wave> waves, int initialIncome)
        {
            _waves = waves;
            _initialIncome = initialIncome;
        }
    }
}