using System;
using FSM;

namespace Buildings.States
{
    public class PlacingState : FsmState
    {
        private readonly Tower _tower;

        public PlacingState(Tower tower) : base(tower)
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
