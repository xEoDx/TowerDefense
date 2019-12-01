using System;
using UnityEngine;

namespace FSM
{
    public abstract class FsmState
    {
        protected readonly Component component;

        protected FsmState(Component component)
        {
            this.component = component;
        }

        public abstract void Init();
        public abstract Type Execute();
        public abstract void Exit();
    }
}
