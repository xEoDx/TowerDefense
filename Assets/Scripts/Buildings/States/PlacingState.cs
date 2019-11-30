using System;

namespace Buildings.States
{
    public class PlacingState : FSMState
    {
        private Tower _tower;

        public PlacingState(Tower tower) : base(tower.gameObject)
        {
            _tower = tower;
        }

        public override void Init()
        {
        }

        public override Type Execute()
        {
            if (_tower.IsPlaced)
            {
                return typeof(RadarState);
            }
            return typeof(PlacingState);
        }

        public override void Exit()
        {
        }
    }
}
