using System;
using System.Collections.Generic;
using DaltonLima.Core;
using State.States;
using UnityEngine;

namespace State
{
    /// <inheritdoc />
    /// <summary>
    /// Handles the State Machine.
    /// The states are managed using a stack.
    /// </summary>
    public class StateManager : Singleton<StateManager>
    {

#region Delegates

        public delegate void StateChanged(State newState);

        /// <summary>
        /// The new current State.
        /// </summary>
        public static event StateChanged OnStateChanged;

        public delegate void StateExit(State exitState);
        /// <summary>
        /// The State that was exited
        /// </summary>
        public static event StateExit OnStateExit;

#endregion Delegates

        private readonly Stack<State> _states = new Stack<State>();

        /// <summary>
        /// Returns the event on the top of the stack, null otherwise.
        /// </summary>
        public State CurrentState => _states.Count > 0 ? _states.Peek() : null;

        private void Start()
        {
            PushState(Onboarding.Instance);
        }

        /// <summary>
        /// Calls OnExit on the current state (without Popping it), then Add a new one.
        /// </summary>
        internal void PushState(State state)
        {
            if (state == null)
            {
                Debug.LogWarning("[StateManager] Trying to Push a null state");
                return;
            }

            if (_states.Count > 0)
            {
                _states.Peek().OnExit();
                OnStateExit?.Invoke(_states.Peek());
            }

            _states.Push(state);
            State.InitStateManager(this);
            _states.Peek().OnEnter();
            OnStateChanged?.Invoke(state);
        }

        /// <summary>
        /// Set the current state
        /// </summary>
        internal void SetState(State state)
        {
            if (state == null)
            {
                Debug.LogWarning("[StateManager] Trying to Set a null state");
                return;
            }

            if (_states.Count > 0)
            {
                _states.Peek().OnExit();
                OnStateExit?.Invoke(_states.Peek());
                _states.Pop();
            }

            _states.Push(state);
            State.InitStateManager(this);
            _states.Peek().OnEnter();
            OnStateChanged?.Invoke(state);
        }

        /// <summary>
        /// Delete the current state, and activate the one bellow that.
        /// </summary>
        internal void PopState()
        {
            if (_states.Count < 2 ) throw new StackOverflowException();

            _states.Peek().OnExit();
            OnStateExit?.Invoke(_states.Peek());
            _states.Pop();

            var currentState = _states.Peek();
            State.InitStateManager(this);
            currentState.OnEnter();
            OnStateChanged?.Invoke(currentState);
        }
    }
}
