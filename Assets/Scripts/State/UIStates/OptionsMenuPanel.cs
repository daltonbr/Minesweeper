using UnityEngine;
using UnityEngine.UI;

#if UNITY_EDITOR
using UnityEngine.Assertions;
#endif

namespace State.UIStates
{
    public class OptionsMenuPanel : UIState
    {
        [SerializeField] private RectTransform panel;
        [Space]
        [Header("Buttons")]
        [SerializeField] private Button closeButton;

        // Delegates
        public delegate void ButtonPress();
        public static event ButtonPress OnCloseButton;

        private void Awake()
        {
#if UNITY_EDITOR

            Assert.IsNotNull(closeButton);
            Assert.IsNotNull(panel);
#endif
            panel.gameObject.SetActive(false);
        }
        
        private static void HandleCloseButton()
        {
            OnCloseButton?.Invoke();
        }

        public override void OnEnter()
        {
            closeButton.onClick.AddListener(HandleCloseButton);
            panel.gameObject.SetActive(true);
        }

        public override void OnExit()
        {
            closeButton.onClick.RemoveAllListeners();
            panel.gameObject.SetActive(false);
        }
    }
}