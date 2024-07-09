using System;
using System.Collections.Generic;
using System.Globalization;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

using Button = UnityEngine.UI.Button;

namespace Calc.Presentation.Calculator.MainWindowMVP
{
    [AddComponentMenu("Game/Main Window/Main Window View")]
    internal sealed class View : MonoBehaviour
    {
        [SerializeField] private Button _evaluate = default!;
        [SerializeField] private Button _clear = default!;
        [SerializeField] private TMP_InputField _inputField = default!;
        [SerializeField] private ScrollRect _evaluatedListView = default!;
        [Header("Evaluated list configuration")]
        [SerializeField] private TMP_Text _evaluatedExpressionPrefab = default!;
        [SerializeField] private Transform _evaluatedListContent = default!;

        private bool _evaluatedListEnabled;

        public event Action<string>? CalculationsRequested;
        public event Action? CloseRequested;

        private void OnEnable()
        {
            _evaluate.onClick.AddListener(OnEvaluateClicked);
            _clear.onClick.AddListener(OnClearClicked);
        }

        private void OnDisable()
        {
            _clear.onClick.RemoveListener(OnClearClicked);
            _evaluate.onClick.RemoveListener(OnEvaluateClicked);
        }

        public void UpdateInputField(decimal value)
        {
            _inputField.text = value.ToString(CultureInfo.InvariantCulture);
        }

        public void UpdateEvaluatedList(string sessionPeekExpression)
        {
            AddNewTextFieldInList(sessionPeekExpression);

            _evaluatedListView.GraphicUpdateComplete();

            EvaluatedListEnable();
        }

        public void UpdateEvaluatedList(IEnumerable<string> expressions)
        {
            using var iterator = expressions.GetEnumerator();

            if (iterator.MoveNext() is false) return;

            EvaluatedListEnable();

            do
            {
                AddNewTextFieldInList(iterator.Current!);
            }
            while (iterator.MoveNext());
        }

        public void ClearEvaluatedList()
        {
            for (var current = 0; current != _evaluatedListContent.childCount; current++)
            {
                Destroy(_evaluatedListContent.GetChild(current).gameObject);
            }
        }

        private void OnEvaluateClicked()
        {
            CalculationsRequested?.Invoke(_inputField.text);
            _inputField.DeactivateInputField(clearSelection: true);
            _inputField.ActivateInputField();
        }

        private void OnClearClicked()
        {
            CloseRequested?.Invoke();

            ClearInputField();
            ClearEvaluatedList();

            EvaluatedListDisable();
        }

        public void ClearInputField()
        {
            _inputField.DeactivateInputField(clearSelection: true);
            _inputField.ActivateInputField();
        }

        private void EvaluatedListEnable()
        {
            if (_evaluatedListEnabled) return;

            _evaluatedListEnabled = true;

            _evaluatedListView.gameObject.SetActive(true);
            _clear.gameObject.SetActive(true);
        }

        private void EvaluatedListDisable()
        {
            if (_evaluatedListEnabled is false) return;

            _evaluatedListEnabled = false;

            _evaluatedListView.gameObject.SetActive(false);
            _clear.gameObject.SetActive(false);
        }

        private void AddNewTextFieldInList(string sessionPeekExpression)
        {
            var candidate = Instantiate(_evaluatedExpressionPrefab, _evaluatedListContent);
            candidate.text = sessionPeekExpression;
            candidate.ForceMeshUpdate(forceTextReparsing: true);
            candidate.SetNativeSize();
            candidate.transform.SetSiblingIndex(0);
        }
    }
}
