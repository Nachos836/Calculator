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

        public event Action<string>? CalculationsRequested;
        public event Action? CloseRequested;

        private void Awake()
        {
            _evaluatedListView.gameObject.SetActive(false);
            _clear.gameObject.SetActive(false);
        }

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

        public void ClearInputField()
        {
            _inputField.DeactivateInputField(clearSelection: true);
            _inputField.ActivateInputField();
        }

        private void EvaluatedListEnable()
        {
            _evaluatedListView.gameObject.SetActive(true);
            _clear.gameObject.SetActive(true);
        }

        private void AddNewTextFieldInList(string sessionPeekExpression)
        {
            var candidate = Instantiate(_evaluatedExpressionPrefab, _evaluatedListContent);
            candidate.text = sessionPeekExpression;
            candidate.ForceMeshUpdate(forceTextReparsing: true);
            candidate.SetNativeSize();
            candidate.transform.SetSiblingIndex(0);
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

            _evaluatedListView.gameObject.SetActive(false);
            _clear.gameObject.SetActive(false);
        }
    }
}
