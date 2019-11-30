using System;
using UnityEngine;

namespace Buildings.States
{
    public class PlacingState : FSMState
    {    
        

        public PlacingState(Tower tower) : base(tower.gameObject)
        {
        }

        public override void Init()
        {
            throw new NotImplementedException();
        }

        public override Type Execute()
        {
            throw new NotImplementedException();
        }

        public override void Exit()
        {
            throw new NotImplementedException();
        }
    }
}
