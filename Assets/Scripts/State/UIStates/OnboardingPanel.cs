using UnityEngine;

#if UNITY_EDITOR
using UnityEngine.Assertions;
#endif

namespace State.UIStates
{
    public class OnboardingPanel : UIState
    {
        [SerializeField] private RectTransform panel;
        
        private void Awake()
        {
#if UNITY_EDITOR
            Assert.IsNotNull(panel);
#endif
            panel.gameObject.SetActive(false);
        }

        public override void OnEnter()
        {
            panel.gameObject.SetActive(true);
        }

        public override void OnExit()
        {
            panel.gameObject.SetActive(false);
        }
    }
}
