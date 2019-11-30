using System.Collections.Generic;

namespace Gameplay
{
    public struct Level
    {
        private IList<Wave> _waves;
        public IList<Wave> Waves => _waves;

        public Level(IList<Wave> waves)
        {
            _waves = waves;
        }
    }
}