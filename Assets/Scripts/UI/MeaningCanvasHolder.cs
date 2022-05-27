using System.Collections;
using Interactable_Objects;
using Managers;
using TMPro;
using UnityEngine;

namespace UI
{
    public class MeaningCanvasHolder : MonoBehaviour
    {
        [SerializeField]
        private float delay = 0.1f;

        private TextMeshProUGUI _myText;
        private int _letterCount = 0;
        private string _meaningString;
        private bool _startAnimation = false;


        public string MeaningString
        {
            get => _meaningString;
            set => _meaningString = value;
        }

        private void Start()
        {
            _myText = GetComponent<TextMeshProUGUI>();
        }

        public void FoundMeaning(MeaningDescriptor sender, InteractableObject e)
        {
            _meaningString = sender.meaning;
            StartCoroutine(WriteLetters());
        }

        IEnumerator WriteLetters()
        {
            while (true)
            {
                if (CanvasManager.ActiveCanvas.Angle < 0)
                {
                    yield return new WaitForSeconds(delay);
                }

                if (_letterCount < _meaningString.Length && CanvasManager.ActiveCanvas.Angle >= 0)
                {
                    if (_letterCount == 0)
                    {
                        CanvasManager.WritingWord = true;
                    }

                    _myText.text += _meaningString[_letterCount++];
                    yield return new WaitForSeconds(delay);
                }

                if (_letterCount >= _meaningString.Length)
                {
                    CanvasManager.WritingWord = false;
                    break;
                }
            }
        }
    }
}