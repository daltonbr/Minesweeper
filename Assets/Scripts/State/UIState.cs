using UnityEngine;

namespace State
{
    /// <summary>
    /// Handles UI OnEnter and OnExit States
    /// </summary>
    public abstract class UIState : MonoBehaviour, IState
    {
        //TODO: Implement specific methods to animate menus automatically when entering/exiting states
        public abstract void OnEnter();
        public abstract void OnExit();
        
    }
}
