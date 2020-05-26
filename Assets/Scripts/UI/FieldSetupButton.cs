using Core;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    [RequireComponent(typeof(Button))]
    public class FieldSetupButton : MonoBehaviour
    {
        [SerializeField] private FieldSetup fieldSetup;
        [SerializeField] private TMP_Text buttonText;
        private Button _button;

        public delegate void FieldSetupClicked(FieldSetup _fieldSetup);
        public event FieldSetupClicked OnFieldSetupClicked;
        
        private void Awake()
        {
            _button = GetComponent<Button>();
            _button.onClick.AddListener(HandleButtonClick);
        }

        private void OnDestroy()
        {
            _button.onClick.RemoveAllListeners();
        }

        private void HandleButtonClick()
        {
            OnFieldSetupClicked?.Invoke(fieldSetup);
        }

        public void Init(FieldSetup _fieldSetup)
        {
            fieldSetup = _fieldSetup;
            buttonText.text = $"{_fieldSetup.size.x.ToString()}x{_fieldSetup.size.y.ToString()}\n{_fieldSetup.configName}";
        }
        
    }
}
