using Core;
using Managers;
using UnityEngine;
using UnityEngine.UI;
using UI;

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
        [SerializeField] private Button optionsButton;

        [Header("Field Setup Buttons")]
        [SerializeField] private GameObject fieldSetupButtonPrefab;
        [SerializeField] private RectTransform fieldSetupPanel;
        
        // Delegates
        public delegate void ButtonPress();
        public static event ButtonPress OnOptionsButton;
        public delegate void FieldSetupClicked(FieldSetup fieldSetup);
        public static event FieldSetupClicked OnFieldSetupClicked;

        public override void OnEnter()
        {
            panel.gameObject.SetActive(true);
            optionsButton.onClick.AddListener(HandleOptionsButton);
        }

        public override void OnExit()
        {
            panel.gameObject.SetActive(false);
            optionsButton.onClick.RemoveAllListeners();
        }

        private void Awake()
        {
#if UNITY_EDITOR
            Assert.IsNotNull(panel);
#endif
            panel.gameObject.SetActive(false);
            GenerateFieldSetupButtons();
        }

        /// <summary>
        /// Instantiate FieldSetup buttons (the difficult buttons)
        /// </summary>
        private void GenerateFieldSetupButtons()
        {
            var setups = GameManager.Instance.fieldSetups;
            foreach (var setup in setups)
            {
                var gameObj = Instantiate(fieldSetupButtonPrefab, fieldSetupPanel);
                var fieldSetup = gameObj.GetComponent<FieldSetupButton>();
                if (fieldSetup == null)
                {
                    Debug.LogWarning("[MainMenuPanel] Couldn't find locate FieldSetup in prefab");
                    return;
                }
                fieldSetup.Init(setup);
                fieldSetup.OnFieldSetupClicked += HandleFieldSetupClicked;
            }
        }

        private static void HandleFieldSetupClicked(FieldSetup _fieldsetup)
        {
            OnFieldSetupClicked?.Invoke(_fieldsetup);
        }
        
        private static void HandleOptionsButton()
        {
            OnOptionsButton?.Invoke();
        }

    }
}
