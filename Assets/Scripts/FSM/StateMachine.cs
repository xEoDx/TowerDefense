using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;

namespace FSM
{
    public class StateMachine : MonoBehaviour
    {
        private IDictionary<Type, FSMState> _states;
        private FSMState _currentState;

        public event Action<FSMState> OnStateChanged;

        public void SetStates(Dictionary<Type, FSMState> states)
        {
            _states = states;
        }

        private void Update()
        {
            if (_currentState == null && _states.Count > 0)
            {
                var initialState = _states.Keys.First();
                ChangeState(initialState);
            }

            var nextState = _currentState?.Execute();

            if (nextState != null && nextState != _currentState?.GetType())
            {
                ChangeState(nextState);
            }
        }
        
        private void ChangeState(Type newState)
        {
            Assert.IsNotNull(newState);
            _currentState?.Exit();
            _currentState = _states[newState];
            OnStateChanged?.Invoke(_currentState);
            _currentState.Init();
            
        }
    }
}
