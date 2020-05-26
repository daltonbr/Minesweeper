using State.UIStates;
using UnityEngine;

namespace State.States
{
    internal class OptionsMenu : State
    {
        public static OptionsMenu Instance { get; } = new OptionsMenu();

        public override void OnEnter()
        {
            Debug.Log("[OptionsMenu] OnEnter");
            OptionsMenuPanel.OnCloseButton += HandleCloseButton;
        }

        public override void OnExit()
        {
            Debug.Log("[OptionsMenu] OnExit");
            OptionsMenuPanel.OnCloseButton -= HandleCloseButton;
        }

        private static void HandleCloseButton()
        {
            StateManager.PopState();
        }
        
    }
}
