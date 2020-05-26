namespace State
{
    /// <summary>
    /// Base class for the States for the State Machine
    /// </summary>
    public abstract class State : IState
    {
        protected static StateManager StateManager;

        public static void InitStateManager(StateManager stateManager)
        {
            StateManager = stateManager;
        }

        public abstract void OnEnter();
        public abstract void OnExit();
    }
}
