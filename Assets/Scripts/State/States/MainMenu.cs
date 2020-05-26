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
            
            MainMenuPanel.OnPlayButton += HandlePlayButton;
            MainMenuPanel.OnOptionsButton += HandleOptionsButton;
        }

        public override void OnExit()
        {
            Debug.Log("[MainMenu] OnExit");
            
            MainMenuPanel.OnPlayButton -= HandlePlayButton;
            MainMenuPanel.OnOptionsButton -= HandleOptionsButton;
        }

        private static void HandlePlayButton()
        {
            StateManager.SetState(GamePlay.Instance);
        }

        private static void HandleOptionsButton()
        {
            StateManager.PushState(OptionsMenu.Instance);
        }
    }
}
