using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Calc.Presentation.Calculator.ErrorPopupMVP
{
    internal sealed class View : MonoBehaviour
    {
        [SerializeField] private Button _submit = default!;
        [SerializeField] private TMP_Text _errorDetails = default!;

        public event Action? SubmitPressed;

        private void OnEnable() => _submit.onClick.AddListener(OnSubmitPressed);
        private void OnDisable() => _submit.onClick.RemoveListener(OnSubmitPressed);

        public void UpdateErrorDescription(string message)
        {
            _errorDetails.text = message;
            _errorDetails.ForceMeshUpdate();
        }

        private void OnSubmitPressed()
        {
            SubmitPressed?.Invoke();
        }
    }
}
