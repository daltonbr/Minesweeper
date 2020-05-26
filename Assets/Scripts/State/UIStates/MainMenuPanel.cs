using UnityEngine;
using UnityEngine.UI;

#if UNITY_EDITOR
using UnityEngine.Assertions;
#endif

namespace State.UIStates
{
    public class MainMenuPanel : UIState
    {
        [SerializeField] private RectTransform panel;
        [Space]
        [Header("Buttons")]
        [SerializeField] private Button playButton;
        [SerializeField] private Button optionsButton;

        // Delegates
        public delegate void ButtonPress();
        public static event ButtonPress OnPlayButton;
        public static event ButtonPress OnOptionsButton;

        public override void OnEnter()
        {
            panel.gameObject.SetActive(true);

            playButton.onClick.AddListener(HandlePlayButton);
            optionsButton.onClick.AddListener(HandleOptionsButton);
        }

        public override void OnExit()
        {
            panel.gameObject.SetActive(false);
            
            playButton.onClick.RemoveAllListeners();
            optionsButton.onClick.RemoveAllListeners();
        }

        private void Awake()
        {
#if UNITY_EDITOR
            Assert.IsNotNull(playButton);
            Assert.IsNotNull(panel);
#endif
            panel.gameObject.SetActive(false);
        }
        
        private static void HandlePlayButton()
        {
            OnPlayButton?.Invoke();
        }

        private static void HandleOptionsButton()
        {
            OnOptionsButton?.Invoke();
        }

    }
}
