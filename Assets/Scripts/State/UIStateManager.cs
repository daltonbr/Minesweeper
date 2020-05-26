using System;
using DaltonLima.Core;
using State.States;
using State.UIStates;
using UnityEngine;

#if UNITY_EDITOR
using UnityEngine.Assertions;
#endif

namespace State
{
    /// <inheritdoc />
    /// <summary>
    /// Singleton class to handle State transitions for the UI
    /// </summary>
    public class UIStateManager : Singleton<UIStateManager>
    {
        [SerializeField] private OnboardingPanel onboardingPanel;
        [SerializeField] private MainMenuPanel mainMenuPanel;
        [SerializeField] private GamePlayPanel gamePlayPanel;
        [SerializeField] private OptionsMenuPanel optionsMenuPanel;
        
        private UIState _currentState;

        private void Awake()
        {
#if UNITY_EDITOR
            Assert.IsNotNull(onboardingPanel);
            Assert.IsNotNull(mainMenuPanel);
            Assert.IsNotNull(gamePlayPanel);
            Assert.IsNotNull(optionsMenuPanel);
#endif
        }

        private void OnEnable()
        {
            StateManager.OnStateChanged += HandleStateChange;
        }

        private void OnDisable()
        {
            StateManager.OnStateChanged -= HandleStateChange;
        }

        private void HandleStateChange(State newState)
        {
            if (_currentState != null)
            {
                _currentState.OnExit();
            }

            _currentState = GetNewState(newState);

            if (_currentState == null) return;

            _currentState.OnEnter();
        }

        private UIState GetNewState(State newState)
        {
            UIState selectedPanel = null;
            switch (newState)
            {
                case Onboarding _:
                    Debug.Log("[UIStateManager] Entering Onboarding state");
                    selectedPanel = onboardingPanel;
                    break;
                case MainMenu _:
                    Debug.Log("[UIStateManager] Entering MainMenu state");
                    selectedPanel = mainMenuPanel;
                    break;
                case GamePlay _:
                    Debug.Log("[UIStateManager] Entering GamePlay state");
                    selectedPanel = gamePlayPanel;
                    break;
                case OptionsMenu _:
                    Debug.Log("[UIStateManager] Entering OptionsMenu state");
                    selectedPanel = optionsMenuPanel;
                    break;
                default:
                    Debug.LogError($"[UIStateManager] Unhandled state {newState}");
                    throw new ArgumentOutOfRangeException(nameof(newState));
            }

            return selectedPanel;
        }
    }
}
