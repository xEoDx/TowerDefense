using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;

namespace FSM
{
    public class StateMachine : MonoBehaviour
    {
        private IDictionary<Type, FsmState> _states;
        private FsmState _currentState;
        private Type _initialStateType;

        public event Action<FsmState> OnStateChanged;

        public void SetStates(Dictionary<Type, FsmState> states, Type initialState = null)
        {
            _states = states;
            _initialStateType = initialState;
        }

        private void Update()
        {
            if (_currentState == null && _states.Count > 0)
            {
                if (_initialStateType == null)
                {
                    _initialStateType = _states.Keys.First();
                }
                    
                ChangeState(_initialStateType);
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
