using Core;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

#if UNITY_EDITOR
using UnityEngine.Assertions;
#endif

namespace State.UIStates
{
    public class GamePlayPanel : UIState
    {
        [SerializeField] private RectTransform panel;
        [Space]
        [Header("Buttons")]
        [SerializeField] private Button mainMenuButton;
        [SerializeField] private Button optionsButton;
        [Space]
        [Header("GameOver Popup")]
        [SerializeField] private TMP_Text gameOverText;
        [SerializeField] private RectTransform gameOverPanel;
        [SerializeField] private Button resetButton;

        // Delegates
        public delegate void ButtonPress();
        public static event ButtonPress OnMainMenuButton;
        public static event ButtonPress OnOptionsButton;
        public static event ButtonPress OnResetButton;
        
        private void Awake()
        {
#if UNITY_EDITOR
            Assert.IsNotNull(panel);
            Assert.IsNotNull(mainMenuButton);
            Assert.IsNotNull(optionsButton);
            Assert.IsNotNull(resetButton);
#endif
            panel.gameObject.SetActive(false);
        }

        public override void OnEnter()
        {
            panel.gameObject.SetActive(true);
            gameOverPanel.gameObject.SetActive(false);
            mainMenuButton.onClick.AddListener(HandleMainMenuButton);
            optionsButton.onClick.AddListener(HandleOptionsButton);
            resetButton.onClick.AddListener(HandleResetButton);
            
            Field.OnGameLost += HandleGameLost;
            Field.OnGameWon += HandleGameWon;
        }

        public override void OnExit()
        {
            panel.gameObject.SetActive(false);
            gameOverPanel.gameObject.SetActive(false);
            mainMenuButton.onClick.RemoveAllListeners();
            optionsButton.onClick.RemoveAllListeners();
            resetButton.onClick.RemoveAllListeners();
            
            Field.OnGameLost -= HandleGameLost;
            Field.OnGameWon -= HandleGameWon;
        }

        private void HandleMainMenuButton()
        {
            gameOverPanel.gameObject.SetActive(false);
            OnMainMenuButton?.Invoke();
        }

        private void HandleOptionsButton()
        {
            gameOverPanel.gameObject.SetActive(false);
            OnOptionsButton?.Invoke();
        }

        private void HandleResetButton()
        {
            gameOverPanel.gameObject.SetActive(false);
            OnResetButton?.Invoke();
        }
        
        private void HandleGameWon()
        {
            gameOverPanel.gameObject.SetActive(true);
            gameOverText.text = "You Won!";
        }

        private void HandleGameLost()
        {
            gameOverPanel.gameObject.SetActive(true);
            gameOverText.text = "You Lost!";
        }
        
    }
}
