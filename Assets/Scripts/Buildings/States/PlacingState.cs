using System;
using FSM;

namespace Buildings.States
{
    public class PlacingState : FsmState
    {
        private readonly CanonTower _canonTower;

        public PlacingState(CanonTower canonTower) : base(canonTower)
        {
            _canonTower = canonTower;
        }

        public override void Init()
        {
        }

        public override Type Execute()
        {
            if (_canonTower.IsPlaced)
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
