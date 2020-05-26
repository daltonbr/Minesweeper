using Core;
using Managers;
using State.UIStates;
using UnityEngine;

namespace State.States
{
    internal class MainMenu : State
    {
        public static MainMenu Instance { get; } = new MainMenu();

        public override void OnEnter()
        {
            Debug.Log("[MainMenu] OnEnter");
            
            MainMenuPanel.OnOptionsButton += HandleOptionsButton;
            MainMenuPanel.OnFieldSetupClicked += HandleFieldSetupClicked;
        }

        private void HandleFieldSetupClicked(FieldSetup fieldsetup)
        {
            GameManager.Instance.CurrentFieldSetup = fieldsetup;
            StateManager.SetState(GamePlay.Instance);
        }

        public override void OnExit()
        {
            Debug.Log("[MainMenu] OnExit");
            
            MainMenuPanel.OnOptionsButton -= HandleOptionsButton;
        }

        private static void HandleOptionsButton()
        {
            StateManager.PushState(OptionsMenu.Instance);
        }
    }
}
