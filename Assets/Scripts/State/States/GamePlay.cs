using System.Collections;
using Managers;
using State.UIStates;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace State.States
{
    internal class GamePlay : State
    {
        public static GamePlay Instance { get; } = new GamePlay();

        public override void OnEnter()
        {
            Debug.Log("[GamePlay] OnEnter");
            
            LoadGameScene();
            GamePlayPanel.OnMainMenuButton += HandleMainMenuButton;
            GamePlayPanel.OnOptionsButton += HandleOptionsButton;
            GamePlayPanel.OnResetButton += BootstrapField;
        }

        public override void OnExit()
        {
            Debug.Log("[GamePlay] OnExit");
            
            GamePlayPanel.OnMainMenuButton -= HandleMainMenuButton;
            GamePlayPanel.OnOptionsButton -= HandleOptionsButton;
            GamePlayPanel.OnResetButton -= BootstrapField;
        }
        
        private static void LoadGameScene()
        {
            if (SceneManager.GetSceneByName("GameScene").isLoaded == false)
            {
                StateManager.StartCoroutine(LoadGameSceneAsync());
            }
            else
            {
                BootstrapField();
            }
        }

        private static IEnumerator LoadGameSceneAsync()
        {
            AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("GameScene", LoadSceneMode.Additive);
            asyncLoad.completed += operation => HandleLoadSceneAsync(operation);
            
            while (!asyncLoad.isDone)
            {
                Debug.Log($"[GamePlay] - Loading Scene: {asyncLoad.progress.ToString()}%");
                yield return null;
            }
        }

        private static void HandleLoadSceneAsync(AsyncOperation asyncOperation)
        {
            Debug.Log("[GamePlay] Scene is Loaded async");

            BootstrapField();
        }

        private static void HandleMainMenuButton()
        {
            StateManager.SetState(MainMenu.Instance);
        }
        
        private static void HandleOptionsButton()
        {
            StateManager.PushState(OptionsMenu.Instance);
        }

        private static void BootstrapField()
        {
            GameManager.Instance.BootstrapField();
        }
    }
}
